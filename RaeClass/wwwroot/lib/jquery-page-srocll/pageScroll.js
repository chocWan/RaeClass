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
                    $html.addClass("pageIndexUl");
                    $.each(initialize.pageIndexItems, function (index, obj) {
                        var $li_html = $('<li/>');
                        $li_html.addClass("pageIndexLi");
                        if (obj.refElementId) {
                            $li_html.click(function () {
                                $('body,html,nav.top_nav ~ .section').animate({
                                    scrollTop: $("#" + obj.refElementId).offset().top - $("#navbanner").get(0).offsetHeight + 1
                                }, 500);
                                //$(this).siblings().each(function (index, item) { $(item).removeClass("pageIndexActive"); });
                                //$(this).addClass("pageIndexActive");
                            })
                        }
                        $li_html.attr("id", obj.refElementId + "_index");
                        $li_html.append("<span>" + obj.indexName + "</span>");
                        if (index == 0) $li_html.css("margin-top","0px");
                        $li_html.appendTo($ul);
                    });
                    $ul.appendTo($html);
                    $(this).append($html);
                    $(window).scroll(function () {
                        //为了保证兼容性，这里取两个值，哪个有值取哪一个
                        //scrollTop就是触发滚轮事件时滚轮的高度
                        var scrollTop = document.documentElement.scrollTop;
                        console.log(scrollTop);
                        var pageIndexs = [];
                        $.each(initialize.pageIndexItems, function (index, item) {
                            pageIndexs.push({ indexId: "#" + item.refElementId + "_index", top: $("#" + item.refElementId).offset().top - $("#navbanner").get(0).offsetHeight - scrollTop});
                        });
                        this.console.log(pageIndexs);
                        var tempRes = null;
                        $.each(pageIndexs, function (index, item) {
                            if (0 >= item.top) {
                                if (tempRes == null) {
                                    tempRes = item;
                                } else {
                                    if (item.top - tempRes.top > 0) {
                                        tempRes = item;
                                    }
                                }
                            }
                        });
                        if (tempRes != null) {
                            $(tempRes.indexId).siblings().each(function (index, item) { $(item).removeClass("pageIndexActive"); });
                            $(tempRes.indexId).addClass("pageIndexActive");
                        }
                    })
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




