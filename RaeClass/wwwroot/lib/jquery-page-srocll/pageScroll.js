(function ($) {
    $.fn.pageIndexing = function (method) {
        var initialize = {};
        var methods = {
            init: function () {
                methods.destroy.apply(this);
                methods.update.apply(this, initialize);
            },
            destroy: function () {
                $(this).find(".pageIndex").remove();
            },
            update: function (obj) {
                var $html = $("<div/>");
                var $ul = $("<ul/>");
                methods.destroy.apply(this);
                if (initialize.state || initialize.moveState) {
                    $html.addClass("pageIndex");
                    $.each(initialize.pageIndexItems, function (index, obj) {
                        var $li_html = $('<li/>');
                        if (obj.refElementId) {
                            $li_html.click(function () {
                                $('body,html,nav.top_nav ~ .section').animate({
                                    scrollTop: $("#" + refElementId).offset().top
                                }, 500);
                            })
                        }
                        $li_html.val(obj.indexName);
                        $li_html.appendTo($ul);
                    });
                    $ul.appendTo($html);
                    $(this).append($html);
                }
            }
        }
        if (methods[method]) {
            return methods[method].apply(this, Array.prototype.slice.call(arguments, 1));
        } else if (typeof method === 'object' || !method) {
            initialize = $.extend(initialize, method);
            return methods.init.apply(this, initialize);
        }
    };
}
)(jQuery);

