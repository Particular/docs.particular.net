(function(i,s,o,g,r,a,m){i['GoogleAnalyticsObject']=r;i[r]=i[r]||function(){
(i[r].q=i[r].q||[]).push(arguments)},i[r].l=1*new Date();a=s.createElement(o),
m=s.getElementsByTagName(o)[0];a.async=1;a.src=g;m.parentNode.insertBefore(a,m)
})(window,document,'script','//www.google-analytics.com/analytics.js','ga');
ga('create', 'UA-20451773-2');
ga('set', 'allowAdFeatures', false);
ga('set', 'transport', 'beacon');

(function ($, window) {
    'use strict';
    window.SOLUTION_VERSION = 'VS2017';

    $(function () {

        var gaEvt = function (category, action, label, callback) {
            var cb = callback || $.noop;
            window.ga('send', 'event', category, action, label, { hitCallback: cb });
            if (!ga.create) {
                cb();
            }
        };

        gaEvt('QuickStart', 'Running', SOLUTION_VERSION);
        window.sentMessage = function () {
            gaEvt('QuickStart', 'SentMessage', SOLUTION_VERSION);
        };

        var form = $('#license-form'),
            firstname = $('#firstname'),
            lastname = $('#lastname'),
            email = $('#email');

        if (!form.length) {
            return;
        }

        var isEmail = function (email) {
            var regex = /^([a-zA-Z0-9_\.\-\+])+\@(([a-zA-Z0-9\-])+\.)+([a-zA-Z0-9]{2,4})+$/;
            return regex.test(email);
        };

        gaEvt('QuickStart', 'DisplayedLicenseForm', SOLUTION_VERSION);

        $("#submit-license").click(function (e) {

            e.preventDefault();
            var formOK = true;

            var check = function (sel, additionalFn) {
                if (sel.val() === "") {
                    sel.attr('placeholder', 'Field is missing');
                    formOK = false;
                } else {
                    sel.removeAttr('placeholder');
                    additionalFn && additionalFn(sel);
                }
            };

            check(firstname);
            check(lastname);
            check(email, function (sel) {
                if (!isEmail(sel.val())) {
                    sel.val("");
                    sel.attr("placeholder", "Email isn't valid");
                    formOK = false;
                } else {
                    sel.removeAttr("placeholder");
                }
            });

            if (!formOK) {
                return;
            }

            var postData = {
                FirstName: firstname.val(),
                LastName: lastname.val(),
                Email: email.val(),
                templateid: "FreeLicense"
            };

            if (window.NSB_VERSION && !!window.NSB_VERSION.match(/^\d+\.\d+\.\d+$/)) {
                postData.NServiceBusVersion = window.NSB_VERSION;
            }

            gaEvt('QuickStart', 'SubmitLicenseForm', SOLUTION_VERSION);

            $.ajax({
                url: "https://api.particular.net/platform/licensetool/requestextension",
                type: "POST",
                data: postData,
                complete: function (xhr, textStatus) {
                    if (textStatus === "success") {

                        form.append($('<input/>', { type: 'hidden', id: 'LeadCategory', name: 'LeadCategory', value: 'Download' }))
                            .append($('<input/>', { type: 'hidden', id: 'LeadSource', name: 'LeadSource', value: 'QuickStartTutorial' }))
                            .append($('<input/>', { type: 'hidden', name: 'version', value: 'V7' }));

                        gaEvt('Action Performed', 'Trial Extension', 'Day 14', function () {
                            form.submit();
                        });
                    }
                    else {
                        alert("Unable to send request due to exception. Please contact Particular at support@particular.net");
                    }
                }
            });
        });

    });

}(jQuery, window));