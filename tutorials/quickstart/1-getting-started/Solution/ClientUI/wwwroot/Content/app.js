(function(i,s,o,g,r,a,m){i['GoogleAnalyticsObject']=r;i[r]=i[r]||function(){
(i[r].q=i[r].q||[]).push(arguments)},i[r].l=1*new Date();a=s.createElement(o),
m=s.getElementsByTagName(o)[0];a.async=1;a.src=g;m.parentNode.insertBefore(a,m)
})(window,document,'script','//www.google-analytics.com/analytics.js','ga');
ga('create', 'UA-20451773-2');
ga('set', 'allowAdFeatures', false);
ga('set', 'transport', 'beacon');

(function ($, window) {
    'use strict';
    window.SOLUTION_VERSION = 'VS2019';

    $(function () {

        var gaEvt = function (category, action, label) {
            console.log('GA', category, action, label);
            window.ga('send', 'event', category, action, label);
        };

        gaEvt('QuickStart', 'Running', SOLUTION_VERSION);
        window.sentMessage = function () {
            gaEvt('QuickStart', 'SentMessage', SOLUTION_VERSION);
        };

        var licenseBtn = $('#license-btn');

        if (!licenseBtn.length) {
            return;
        }

        gaEvt('QuickStart', 'DisplayedLicenseButton', SOLUTION_VERSION);

        licenseBtn.attr('href', 'https://particular.net/license/nservicebus?v=' + window.NSB_VERSION + '&t=0').click(function (e) {

            gaEvt('QuickStart', 'ClickedLicenseButton', SOLUTION_VERSION);

        });

    });

}(jQuery, window));