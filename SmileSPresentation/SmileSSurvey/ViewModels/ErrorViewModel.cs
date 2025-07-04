using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SmileSSurvey.ViewModels
{
    public class ErrorViewModel
    {
        public ErrorMessage Error { get; set; }
    }

    public class ErrorMessage
    {
        //
        // Summary:
        //     The display mode passed from the authorization request.
        //
        // Value:
        //     The display mode.
        public string DisplayMode { get; set; }

        //
        // Summary:
        //     The UI locales passed from the authorization request.
        //
        // Value:
        //     The UI locales.
        public string UiLocales { get; set; }

        //
        // Summary:
        //     Gets or sets the error code.
        //
        // Value:
        //     The error code.
        public string Error { get; set; }

        //
        // Summary:
        //     Gets or sets the error description.
        //
        // Value:
        //     The error description.
        public string ErrorDescription { get; set; }

        //
        // Summary:
        //     The per-request identifier. This can be used to display to the end user and can
        //     be used in diagnostics.
        //
        // Value:
        //     The request identifier.
        public string RequestId { get; set; }

        //
        // Summary:
        //     The redirect URI.
        public string RedirectUri { get; set; }

        //
        // Summary:
        //     The response mode.
        public string ResponseMode { get; set; }

        //
        // Summary:
        //     The client id making the request (if available).
        public string ClientId { get; set; }
    }
}