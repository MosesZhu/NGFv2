jQuery.extend({
    browser: function () {
        var
            rwebkit = /(webkit)\/([\w.]+)/,
            ropera = /(opera)(?:.*version)?[ \/]([\w.]+)/,
            rmsie = /(msie) ([\w.]+)/,
            rmozilla = /(mozilla)(?:.*? rv:([\w.]+))?/,
            browser = {},
            ua = window.navigator.userAgent,
            browserMatch = uaMatch(ua);

        if (browserMatch.browser) {
            browser[browserMatch.browser] = true;
            browser.version = browserMatch.version;
        }
        return { browser: browser };
    },
});

function uaMatch(ua) {
    ua = ua.toLowerCase();

    var match = rwebkit.exec(ua)
        || ropera.exec(ua)
        || rmsie.exec(ua)
        || ua.indexOf("compatible") < 0 && rmozilla.exec(ua)
        || [];

    return {
        browser: match[1] || "",
        version: match[2] || "0"
    };
}

(function ($) {
    $.fn.wresize = function (f) {
        version = '1.1';
        wresize = {
            fired: false,
            width: 0
        };

        function resizeOnce() {
            if ($.browser.msie) {
                if (!wresize.fired) {
                    wresize.fired = true;
                } else {
                    var version = parseInt($.browser.version, 10);
                    wresize.fired = false;
                    if (version < 7) {
                        return false;
                    } else if (version == 7) {
                        // a vertical resize is fired once, an horizontal resize  
                        // twice  
                        var width = $(window).width();
                        if (width != wresize.width) {
                            wresize.width = width;
                            return false;
                        }
                    }
                }
            }
            return true;
        }

        function handleWResize(e) {
            if (resizeOnce()) {
                return f.apply(this, [e]);
            }
        }

        this.each(function () {
            if (this == window) {
                $(this).resize(handleWResize);
            } else {
                $(this).resize(f);
            }
        });
        return this;
    };
})(jQuery);