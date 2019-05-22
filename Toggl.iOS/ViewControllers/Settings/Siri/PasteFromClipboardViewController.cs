﻿using System;
using CoreGraphics;
using Toggl.Core;
using Toggl.Core.UI.ViewModels.Settings.Siri;
using Toggl.iOS.Extensions;
using Toggl.iOS.Extensions.Reactive;
using Toggl.iOS.Presentation.Attributes;
using Toggl.Shared;
using Toggl.Shared.Extensions;
using UIKit;

namespace Toggl.iOS.ViewControllers.Settings.Siri
{

    [ModalDialogPresentation]
    public partial class PasteFromClipboardViewController : ReactiveViewController<PasteFromClipboardViewModel>
    {
        private readonly int cardHeight = 374;
        private readonly int cardWidth = 288;

        public PasteFromClipboardViewController()
            : base(nameof(PasteFromClipboardViewController))
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            localizeViews();

            PreferredContentSize = new CGSize(
                cardWidth,
                cardHeight
            );

            OkayButton.Rx()
                .BindAction(ViewModel.Ok)
                .DisposedBy(DisposeBag);

            DoNotShowAgainButton.Rx()
                .BindAction(ViewModel.DoNotShowAgain)
                .DisposedBy(DisposeBag);
        }

        private void localizeViews()
        {
            TitleLabel.Text = Resources.SiriClipboardInstructionTitle;
            DescriptionLabel.Text = Resources.SiriClipboardInstructionDescription;
            OkayButton.SetTitle(Resources.Ok, UIControlState.Normal);
            DoNotShowAgainButton.SetTitle(Resources.SiriClipboardInstructionDoNotShowAgain, UIControlState.Normal);
        }
    }
}

