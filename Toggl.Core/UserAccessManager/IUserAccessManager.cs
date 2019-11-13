using System;
using System.Reactive;
using System.Threading.Tasks;
using Toggl.Networking;
using Toggl.Shared;

namespace Toggl.Core.Login
{
    public interface IUserAccessManager
    {
        IObservable<ITogglApi> UserLoggedIn { get; }
        IObservable<Unit> UserLoggedOut { get; }

        void OnUserLoggedOut();
        bool CheckIfLoggedIn();
        string GetSavedApiToken();
        void LoginWithSavedCredentials();

        Task Login(Email email, Password password);
        Task LoginWithGoogle(string googleToken);

        Task SignUp(Email email, Password password, bool termsAccepted, int countryId, string timezone);
        Task SignUpWithGoogle(string googleToken, bool termsAccepted, int countryId, string timezone);

        Task<string> ResetPassword(Email email);
        Task RefreshToken(Password password);
    }
}
