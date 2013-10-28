(function($){
    $.widget("ui.renwuGuanliDialog", {
            options: {
                renwuListUrl: null,
                xiugaiRenwuChulirenUrl: null,
                chengyuanDialog: null
	        },
	        _create: function(){
                var thiz = this;
                this._modal = this.element.find(".modal");
                
                this._renwuGrid = this.element.find(".renwuGrid").datagrid({
		            columns:[
			            {title: "步骤名称", width: 80, field:"mingcheng"},
			            {title: "处理人", field:"chuliren", width: 100, render: function(row, args){
                            if(args.data.zhuangtai == 0){
				                var el = $("<div><span>"+args.value+"</span><a style='margin-left: 5px;' href='#'>设置</a></div>");
                                el.find("a").click(function(){
                                    var renwuId = args.data.guid;
                                    thiz.options.chengyuanDialog.chengyuanDialog("xuanzeYonghu", function(yonghuList){
                                        var yonghu = yonghuList[0];
                                        $.post(thiz.options.xiugaiRenwuChulirenUrl, {renwuId: renwuId, chuliren: yonghu.zhanghao}, function(resultModel){
                                            if(resultModel.result == 0){
                                                thiz.options.chengyuanDialog.chengyuanDialog("guanbi");
                                                thiz._jiazaiRenwuGrid();
                                                alert("设置成功");
                                            }else{
                                                alert(resultModel.message);
                                            }
                                        });
                                    }, true);
                                    return false;
                                });
                                return el;
                            }
                            else{
                                return args.value;
                            }
			            }},
			            {title: "状态", width: 60, field:"zhuangtaiMingcheng"},
			            {title: "开始时间", width: 150, field:"kaishiShijian"},
			            {title: "完成时间", width: 150, field:"wanchengShijian"}
		            ],
		            canSort: false,
		            singleSelect: true,
		            showNumberRow: true
	            });
	        },
            guanli: function(liuchengId){
                this._liuchengId = liuchengId;
                this._jiazaiRenwuGrid();
                this._modal.modal("show");
            },
            _jiazaiRenwuGrid: function(){
                var thiz = this;
                $.get(this.options.renwuListUrl, {liuchengId: this._liuchengId}, function(model){
                    if(model.result == 0){
                        thiz._renwuGrid.datagrid("option", "data", model.data);
                    }
                    else{
                        alert(model.message)
                    }
                });
            }
        }
    );
})(jQuery);