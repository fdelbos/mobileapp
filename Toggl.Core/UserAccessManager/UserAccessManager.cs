using System;
using System.Reactive;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Threading.Tasks;
using Toggl.Core.Models;
using Toggl.Core.Services;
using Toggl.Networking;
using Toggl.Networking.Network;
using Toggl.Shared;
using Toggl.Shared.Models;
using Toggl.Storage;
using Task = System.Threading.Tasks.Task;

namespace Toggl.Core.Login
{
    public sealed class UserAccessManager : IUserAccessManager
    {
        private readonly Lazy<IApiFactory> apiFactory;
        private readonly Lazy<ITogglDatabase> database;
        private readonly Lazy<IPrivateSharedStorageService> privateSharedStorageService;

        private readonly ISubject<ITogglApi> userLoggedInSubject = new Subject<ITogglApi>();
        private readonly ISubject<Unit> userLoggedOutSubject = new Subject<Unit>();

        public IObservable<ITogglApi> UserLoggedIn => userLoggedInSubject.AsObservable();
        public IObservable<Unit> UserLoggedOut => userLoggedOutSubject.AsObservable();

        public UserAccessManager(
            Lazy<IApiFactory> apiFactory,
            Lazy<ITogglDatabase> database,
            Lazy<IPrivateSharedStorageService> privateSharedStorageService)
        {
            Ensure.Argument.IsNotNull(database, nameof(database));
            Ensure.Argument.IsNotNull(apiFactory, nameof(apiFactory));
            Ensure.Argument.IsNotNull(privateSharedStorageService, nameof(privateSharedStorageService));

            this.database = database;
            this.apiFactory = apiFactory;
            this.privateSharedStorageService = privateSharedStorageService;
        }

        public async Task Login(Email email, Password password)
        {
            if (!email.IsValid)
                throw new ArgumentException($"A valid {nameof(email)} must be provided when trying to login");
            if (!password.IsValid)
                throw new ArgumentException($"A valid {nameof(password)} must be provided when trying to login");

            await clearDatabase();
            var credentials = Credentials.WithPassword(email, password);
            var api = apiFactory.Value.CreateApiWith(credentials);
            var user = await api.User.Get();
            await createRecordInDatabase(user);
        }

        public async Task LoginWithGoogle(string googleToken)
        {
            await clearDatabase();
            var credentials = Credentials.WithGoogleToken(googleToken);
            var api = apiFactory.Value.CreateApiWith(credentials);
            var user = await api.User.GetWithGoogle();
            await createRecordInDatabase(user);
        }

        public async Task SignUp(Email email, Password password, bool termsAccepted, int countryId, string timezone)
        {
            if (!email.IsValid)
                throw new ArgumentException($"A valid {nameof(email)} must be provided when trying to signup");
            if (!password.IsValid)
                throw new ArgumentException($"A valid {nameof(password)} must be provided when trying to signup");

            await clearDatabase();
            var api = apiFactory.Value.CreateApiWith(Credentials.None);
            var user = await api.User.SignUp(email, password, termsAccepted, countryId, timezone);
            await createRecordInDatabase(user);
        }

        public async Task SignUpWithGoogle(string googleToken, bool termsAccepted, int countryId, string timezone)
        {
            await clearDatabase();
            var api = apiFactory.Value.CreateApiWith(Credentials.None);
            var user = await api.User.SignUpWithGoogle(googleToken, termsAccepted, countryId, timezone);
            await createRecordInDatabase(user);
        }

        public async Task<string> ResetPassword(Email email)
        {
            if (!email.IsValid)
                throw new ArgumentException($"A valid {nameof(email)} must be provided when trying to reset forgotten password.");

            var api = apiFactory.Value.CreateApiWith(Credentials.None);
            return await api.User.ResetPassword(email);
        }

        public bool CheckIfLoggedIn()
        {
            if (privateSharedStorageService.Value.HasUserDataStored())
                return true;

            try
            {
                var user = database.Value.User.Single().Wait();
                storeApiInfoOnPrivateStorage(user);
            }
            catch
            {
                return false;
            }

            return true;
        }

        public void LoginWithSavedCredentials()
        {
            if (privateSharedStorageService.Value.HasUserDataStored())
            {
                userLoggedInSubject.OnNext(apiFromSharedStorage());
                return;
            }

            try
            {
                var user = database.Value.User.Single().Wait();
                var api = apiFromUser(user);
                userLoggedInSubject.OnNext(api);
            }
            catch
            {
                // silent error
            }
        }

        public string GetSavedApiToken()
            => privateSharedStorageService.Value.GetApiToken();

        public async Task RefreshToken(Password password)
        {
            if (!password.IsValid)
                throw new ArgumentException($"A valid {nameof(password)} must be provided when trying to refresh token");

            var user = await database.Value.User.Single().SingleAsync();
            var credentials = Credentials.WithPassword(user.Email, password);
            var api = apiFactory.Value.CreateApiWith(credentials);
            var userData = await api.User.Get();

            var cleanUser = User.Clean(userData);
            var updatedUser = await database.Value.User.Update(cleanUser);
            var finalApi = apiFromUser(updatedUser);

            storeApiInfoOnPrivateStorage(updatedUser);
            userLoggedInSubject.OnNext(finalApi);
        }

        public void OnUserLoggedOut()
        {
            userLoggedOutSubject.OnNext(Unit.Default);
        }

        private async Task createRecordInDatabase(IUser userData)
        {
            var cleanUser = User.Clean(userData);
            var user = await database.Value.User.Create(cleanUser);
            var api = apiFromUser(user);

            storeApiInfoOnPrivateStorage(user);
            userLoggedInSubject.OnNext(api);
        }

        private void storeApiInfoOnPrivateStorage(IUser user)
        {
            privateSharedStorageService.Value.SaveApiToken(user.ApiToken);
            privateSharedStorageService.Value.SaveUserId(user.Id);
        }

        private ITogglApi apiFromSharedStorage()
        {
            var apiToken = privateSharedStorageService.Value.GetApiToken();
            var credentials = Credentials.WithApiToken(apiToken);
            return apiFactory.Value.CreateApiWith(credentials);
        }

        private ITogglApi apiFromUser(IUser user)
        {
            var credentials = Credentials.WithApiToken(user.ApiToken);
            return apiFactory.Value.CreateApiWith(credentials);
        }

        private async Task clearDatabase()
        {
            await database.Value.Clear().SingleAsync();
        }
    }
}
