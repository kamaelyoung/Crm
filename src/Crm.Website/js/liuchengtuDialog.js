(function($){
    $.widget("ui.liuchengtuDialog", {
            options: {
                zhipaiUrl: null
	        },
	        _create: function(){
                var thiz = this;
                this._modal = this.element.find(".modal");
                this._img = this.element.find("img");
	        },
            xianshi: function(url){
                this._img.attr("src", url + "&" + Math.random());
                this._modal.modal("show");
            }
        }
    );
})(jQuery);