 /******************************************************************************
 The following functions are presented to popup tips or sub-form.
 Author: Anson Lin
 Date:   31-June-2006
 *******************************************************************************/
 
/*<!--Tips提示信息 开始-->*/

// 弹出的窗口的宽度, 100-300 pixels 合适
var wsc_width = "130";
// 边缘的宽度，象素, 1-3 pixels 合适
var wsc_border = "1";
// 弹出窗口位于鼠标左侧或者右侧的距离，象素, 3-12合适
var wsc_offsetx = 10;
// 弹出窗口位于鼠标下方的距离, 3-12合适
var wsc_offsety = 10;

var wsc_x    = 0;
var wsc_y    = 0;
var wsc_snow = 0; //是否已生成
var wsc_dir  = 1; //The position of tip object

var wsc_TipDivStyle = null;

document.onmousemove = wsc_mouseMove;

// 以下是页面中使用的公共函数,Anson Lin
// Simple popup right
function wscShowTips(text) {
   try
    {
       if(text!="") 
       {
            wsc_TipDivStyle = document.getElementById("wscDivTipsSystem").style;

            try
            {
	            document.getElementById("wscTdTipsText").innerText = text;
	            wsc_dir = 1;
	            wsc_disp();
	        }catch(e){}
	    }
	    else
	    {
	        wscHideTips();
	    }
	}catch(e){}
}

// Clears popups if appropriate
function wscHideTips() 
{
	try
	{
		    wsc_snow = 0;
		    wsc_hideObject(wsc_TipDivStyle);	   
	}catch(e){}
}

// 非公共函数，被其它的函数调用


// Common calls
function wsc_disp() {
	try
	{
		if (wsc_snow == 0) 	
		{
			wsc_moveTo(wsc_TipDivStyle);
			
			wsc_showObject(wsc_TipDivStyle);
			wsc_snow = 1;			
	    }
	}catch(e){} 
// Here you can make the text goto the statusbar.
}

// Moves the layer
function wsc_mouseMove(e) {
   try{	    
	    if (wsc_snow) 
	    {		    
			wsc_moveTo(wsc_TipDivStyle);
		}
	}catch(e){}
}

// Make an object visible
function wsc_showObject(obj) {
   try{ obj.visibility = "visible"; }catch(e){}
}
// Hides an object
function wsc_hideObject(obj) {
   try{ obj.visibility = "hidden";  } catch(e){} 
}
// Move a layer,Anson Lin
// Modified by Anson on June 8,2006:  Remove one parameter
function wsc_moveTo(obj) {
    try
    {
       wsc_x = event.x + Math.abs(document.body.getBoundingClientRect().left);  //document.body.scrollLeft; 
       wsc_y = event.y + Math.abs(document.body.getBoundingClientRect().top);   //document.body.scrollTop;

       if (wsc_dir == 2) 
       { // Center
	      xL = wsc_x + wsc_offsetx - (wsc_width / 2);
	      yL = wsc_y + wsc_offsety;
	   }
	   if (wsc_dir == 1) 
	   { // Right
	      xL = wsc_x + wsc_offsetx;
	      yL = wsc_y + wsc_offsety;
	   }
	   if (wsc_dir == 0) 
	   { // Left
	      xL = wsc_x - wsc_offsetx - wsc_width;
	      yL = wsc_y + wsc_offsety;
	   }
			        
       obj.left = xL + "px";
       obj.top = yL + "px";
    } catch(e){}  
}

/*<!--Tips提示信息 结束-->*/

/*======================================================================================================*/

/*<!--悬浮提示信息 开始-->*/

/* Retrieve the specific Url page and show it.*/
//var wsc_FloatShown = 0; //是否已生成

/*显示层并抓出detail内容（参数：激活显示的对象，要显示的URL地址）*/
function wscShowFloatForm(sender,detailUrl,width,height)
{
    try{
        var divObject=document.getElementById("wscFloatForm");
        //if (wsc_FloatShown == 0)	
        //{     
            divObject.style.width  = width;
            divObject.style.height = height;

            wscSetFloatFormPos(sender,divObject); /*根据源对象，设定detail层显示的位置*/
            //divObject.getElementsByTagName("iframe")[0].document.clear();
            divObject.getElementsByTagName("iframe")[0].src=detailUrl; /*让层里面的框架请求该地址的内容*/
            
            divObject.style.visibility="visible";      
            
            //wsc_FloatShown = 1;  
        //}       
    }
    catch(e){}
}

function wscHideFloatForm()
{
    try{      
        var divObject=document.getElementById("wscFloatForm");

        divObject.style.visibility="hidden";
        divObject.getElementsByTagName("iframe")[0].src=""; /*停止进度条*/
        
        //wsc_FloatShown = 0;
    }
    catch(e){}
}

/*设定detail层显示的位置（参数：激活显示的对象，detail层对象）*/
function wscSetFloatFormPos(sender,divObject)
{
    //悬浮框的坐标
	var x;
	var y;
	
	//悬浮框的宽和高
	var intWidth=divObject.getBoundingClientRect().right-divObject.getBoundingClientRect().left;
	var intHeight=divObject.getBoundingClientRect().bottom-divObject.getBoundingClientRect().top;
	
	//上部位置够用
	if (event.y > intHeight)
	{
	    //紧贴鼠标
		x=sender.getBoundingClientRect().right;
		y=sender.getBoundingClientRect().bottom-intHeight;
	}
	else
	{
		x=sender.getBoundingClientRect().right;
		y=0;
	}
	
    //可能跟浏览器有关 by Hedda 20060522
	//divObject.style.left=document.body.scrollLeft + x;
	//divObject.style.top=document.body.scrollTop + y;
	divObject.style.left=Math.abs(document.body.getBoundingClientRect().left) + x + "px";
	divObject.style.top= Math.abs(document.body.getBoundingClientRect().top) + y + "px";
	return;
}

//----------------------------------------------------------------------------------------------------------

/*  每隔0.5s,检查iframe状态是否load完成  */
/*
function checkFloatState()
{    	
	    if(document.all.divPopupFloatDetail.readyState!="complete") {
		    document.all.popupFormLoading.style.display="block";
		    window.setTimeout("checkFloatState()",500);
	    }
	    else {
		    document.all.popupFormLoading.style.display="none";
		    document.all.divPopupFloatDetail.style.display="block";
            / * if(document.all.frmDetail.src!="about:blank" && document.all.frmDetail.src!="")* /
            / * window.top.document.all.bottomRoot.src="<%=strUrl%>";* /
	    }
}
*/
/*<!--悬浮提示信息 结束-->*/