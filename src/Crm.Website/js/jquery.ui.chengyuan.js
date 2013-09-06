(function($){
    $.widget("ui.chengyuan", {
            options: {
                mingcheng: null,
                id: null
	        },
	        _create: function(){
                var thiz = this;
                this.element.css({display: "inline-block", border: "1px solid #eee"});
                this.element.html("<button type='button' class='close'>×</button><span style='padding: .3em;'>"+this.options.mingcheng+"</span>");
                this.element.find("button").click(function () {
                    thiz._trigger("removed", null, this.options);
                    thiz.element.remove();
                });
	        }
        }
    );
})(jQuery);