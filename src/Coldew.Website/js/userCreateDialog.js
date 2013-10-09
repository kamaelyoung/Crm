(function($){
    $.widget("ui.userCreateDialog", {
            options: {
                
	        },
	        _create: function(){
                var thiz = this;
                this._modal = this.element.find(".modal");
                this._createUrl = $.baseUrl + "Org/CreateUser";
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
                        formValue.positionId = thiz._positionId;
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
            create: function(positionId, callback){
                this._positionId = positionId;
                this._createdCallback = callback;
                this._modal.modal("show");
                this.element.find("form").setFormValue({account:"", name: "", password: "123456", email: ""});
            }
        }
    );
})(jQuery);