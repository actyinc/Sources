This project excercises the following platforms, frameworks or features:

    Xamarin.iOS
        Push notifications
    Xamarin.Android
        Push notifications
    Xamarin.Forms
        XAML
        Bindings
        Converters
        Central Styles
        Custom Renderers
        Animations
        IoC
        Messaging Center
        Custom Controls
        MessageBar library
        ImageCircle Plugin
        Connectivity Plugin
    Xamarin Insights
    Xamarin Test Cloud
        UITest
        Extensions
        single code-base for iOS & Android
    Azure Mobile Services
        C# backend
        cross-platform templated push notifications

This project employs a few patterns listed below:

    Enforces a ViewModel-per-Page concept
        all ContentPage classes enforce a generic BaseViewModel type
        automatically set as the binding context
    All tasks are proxied through a RunSafe method
        verifies connectivity
        gracefully handles and reports exceptions
    Leagues are assigned a randomly selected themed color at runtime

Keys

    Default keys have been provided to connect to an existing Azure instance
        You will need to create your own app in Google Developer Console to generate a GCM Sender ID if you wish to test out push notifications
        Insights integration has been abstracted if no valid key is detected - insert your own API key to enable Insights functionality
    If you wish to stand up your own backend, you will need to replace the existing fields in Keys.cs file for Azure, OAuth (Google), Xamarin Insights, Xamarin Test Cloud and Flickr
    To run the included UITest suite, you'll need to provide a test Google email address and password

Notes

    You will need a valid Google account to log into the application
    Parallax feature should be tested on a device - simulator will cause jitter

Copyright and license

    Code and documentation copyright 2015 Xamarin, Inc. Code released under the MIT license.
