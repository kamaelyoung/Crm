(function($){
    $.widget("ui.jianglaiZhipaiDialog", {
            options: {
                zhipaiUrl: null
	        },
	        _create: function(){
                var thiz = this;
                this._modal = this.element.find(".modal");
                this.element.find(".yonghuXuanze").yonghuXuanze({chengyuanDialog: $.chengyuanDialog});
                this.element.find(".date").datepicker();
                this._form = this.element.find("form").eq(0);
                this._form.validate({
                    sendForm : false,
                    onBlur: true,
                    onChange: true,
				    eachValidField : function() {
					    $(this).closest('.control-group').removeClass('error');
				    },
				    eachInvalidField : function() {
					    $(this).closest('.control-group').addClass('error');
				    },
                    valid : function() {
                        var formValue = thiz._form.getFormValue();
                        $.post(thiz.options.zhipaiUrl, formValue, function(resultModel){
                            if(resultModel.result == 0){
                                thiz._modal.modal("hide");
                            }
                            else{
                                alert(resultModel.message);
                            }
                        });
                    }
                });
                
	        },
            zhipai: function(){
                var thiz = this;
                $.get(this.options.zhipaiUrl, null, function(resultModel){
                    if(resultModel.result == 0){
                        if(resultModel.data){
                            thiz._form.setFormValue(resultModel.data);
                        }
                    }
                    else{
                        alert(resultModel.message);
                    }
                });
                this._modal.modal("show");
            }
        }
    );
})(jQuery);