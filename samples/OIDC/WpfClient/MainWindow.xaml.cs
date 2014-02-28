﻿using System;
using System.Collections.Generic;
using System.Windows;
using Thinktecture.IdentityModel.Client;
using Thinktecture.Samples;

namespace WpfClient
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        LoginWebView _login;
        AuthorizeResponse _response;

        public MainWindow()
        {
            InitializeComponent();

            _login = new LoginWebView();
            _login.Done += _login_Done;

            Loaded += MainWindow_Loaded;
        }

        void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            _login.Owner = this;
        }

        void _login_Done(object sender, AuthorizeResponse e)
        {
            _response = e;
            Textbox1.Text = e.Raw;
        }

        private void LoginOnlyButton_Click(object sender, RoutedEventArgs e)
        {
            RequestToken("openid", "id_token");
        }

        private void LoginWithProfileButton_Click(object sender, RoutedEventArgs e)
        {
            RequestToken("openid profile", "id_token");
        }

        private void LoginWithProfileAndAccessTokenButton_Click(object sender, RoutedEventArgs e)
        {
            RequestToken("openid email read write", "id_token token");
        }

        private void AccessTokenOnlyButton_Click(object sender, RoutedEventArgs e)
        {
            RequestToken("read", "token");
        }

        private void RequestToken(string scope, string responseType)
        {
            var additional = new Dictionary<string, string>
            {
                { "nonce", "nonce" }
            };

            var client = new OAuth2Client(new Uri("http://localhost:3333/core/connect/authorize"));
            var startUrl = client.CreateAuthorizeUrl(
                "implicitclient",
                responseType,
                scope,
                "oob://localhost/wpfclient",
                "state",
                additional);


            _login.Show();
            _login.Start(new Uri(startUrl), new Uri("oob://localhost/wpfclient"));
        }

        private void ShowIdTokenButton_Click(object sender, RoutedEventArgs e)
        {
            if (_response.Values.ContainsKey("id_token"))
            {
                var viewer = new IdentityTokenViewer();
                viewer.IdToken = _response.Values["id_token"];
                viewer.Show();
            }
        }
    }
}