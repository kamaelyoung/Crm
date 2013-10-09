(function($){
    $.widget("ui.metadataSelect", {
            options: {
                metadataSelectDialog: null
	        },
	        _create: function(){
                var thiz = this;
                this._btnSelect = this.element.find(".btnSelect");
                this._txtName = this.element.find(".txtName");
                this._txtId = this.element.find(".txtId");
                var valueFormId = this.element.data("formId");
                var valueFormName = this.element.data("formName");
                this._btnSelect.click(function(){
                    thiz.options.metadataSelectDialog.metadataSelectDialog("select", valueFormId, valueFormName, function(args){
                        thiz._txtName.val(args.name);
                        thiz._txtId.val(args.id);
                    });
                    return false;
                });
	        }
        }
    );
})(jQuery);