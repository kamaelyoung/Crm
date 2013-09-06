(function($){
    $.widget("ui.positionCreateDialog", {
            options: {
                
	        },
	        _create: function(){
                var thiz = this;
                this._modal = this.element.find(".modal");
                this._createUrl = $.baseUrl + "Org/CreatePosition";
                this.element.find("form").validate({
                    sendForm : false,
                    onBlur: true,
                    onChange: true,
	                eachValidField : function() {
		                $(this).closest('.control-group').removeClass('error');
                        $(this).next('.help-inline').hide();
	                },
	                eachInvalidField : function() {
		                $(this).closest('.control-group').addClass('error');
                        $(this).next('.help-inline').show();
	                },
                    valid: function(){
                        var formValue = thiz.element.find("form").getFormValue();
                        formValue.parentId = thiz._parentId;
                        $.post(thiz._createUrl, formValue, function(model){
                            if(model.result == 0){
                                thiz._modal.modal("hide");
                                thiz._createdCallback(model.data);
                            }
                            else{
                                alert(model.message)
                            }
                        });
                    }
                });
                
	        },
            create: function(parentId, callback){
                this._parentId = parentId;
                this._createdCallback = callback;
                this._modal.modal("show");
                this.element.find("form").setFormValue({name: ""});
            }
        }
    );
})(jQuery);