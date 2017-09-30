//by Hedda 20081210 增加输出控制参数
//==================================================== 参数设定部分 ======================================================= 
var bMoveable=true; //设置日历是否可以拖动 
var _VersionInfo="Javascript Calendar 1.1 (QCS/RIS13/NPM)" //版本信息 
var WSC_Render_Calendar_Before_Year = 2;
var WSC_Render_Calendar_After_Year =5;

//==================================================== WEB 页面显示部分 ===================================================== 
var strFrame; //存放日历层的HTML代码 
document.writeln('<iframe id=myDateLayer scrolling=no frameborder=no style="background-color: #AEC9E3; position: absolute; width: 224px; height: 192px; z-index: 9998; display: none"></iframe>'); 
strFrame='<style>'; 
strFrame += 'INPUT.button{cursor: hand; background-color:#6699CC; ';//BORDER-RIGHT: #9496E1 1px solid;BORDER-TOP: #9496E1 1px solid;BORDER-LEFT: #9496E1 1px solid;BORDER-BOTTOM: #9496E1 1px solid;'; 
strFrame += 'font-family:Arial;FONT-SIZE: 8pt; FONT-WEIGHT: bold; width:26px; height:20px; color: #ffffff; BORDER-TOP-STYLE: none; BORDER-RIGHT-STYLE: none; BORDER-LEFT-STYLE: none; HEIGHT: 20px; BORDER-BOTTOM-STYLE: none}'; 
strFrame += 'TD{FONT-SIZE: 11pt;font-family:Arial;}'; 
strFrame += '</style>'; 

strFrame += '<style>'; 
strFrame += '.DayNC { width:28px; font-family: Arial; font-size: 11px; background-color: #ffffff; color: #336699; font-weight: bold; border-top: 1px solid #6699CC; }'; 
strFrame += '.DayN {  width:30px; cursor: hand;font-family: Arial; font-size: 11px; background-color: #ffffff; color: #336699; font-weight: bold; text-align: center; vertical-align: middle;border-top: 1px solid #6699CC; }'; 
strFrame += '.DayW {  width:30px; cursor: hand;font-family: Arial; font-size: 11px; background-color: #ffffff; color: #669900; font-weight: bold; text-align: center; vertical-align: middle;border-top: 1px solid #6699CC; }'; 
strFrame += '.DayNU { width:28px; cursor: hand;  font-family: Arial; font-size: 11px; background-color: #ffffff; color: #FF6600; font-weight: bold; text-align: center; vertical-align: middle; border-left: 1px outset #6699CC;border-right: 1px outset #6699CC;border-top:1px outset #6699CC; }'; 
strFrame += '.DayWU { width:28px; cursor: hand;  font-family: Arial; font-size: 11px; background-color: #ffffff; color: #FF6600; font-weight: bold; text-align: center; vertical-align: middle; border-left: 1px outset #6699CC;border-right: 1px outset #6699CC;border-top:1px outset #6699CC; }'; 
strFrame += '.WeekDay{ width:30px; font-family: Arial; font-size: 11px; background-color: #AEC9E3; color: #336699; font-weight: bold; text-align: center; vertical-align: middle; border-top: 2px solid #6699CC; }'; 
strFrame += '.WeekEnd{ width:30px; font-family: Arial; font-size: 11px; background-color: #AEC9E3; color: #669900; font-weight: bold; text-align: center; vertical-align: middle; border-top: 2px solid #6699CC; }'; 
strFrame += '.ArrowN{  width:28px; cursor: hand; font-family: Arial; font-size: 11px; background-color: #6699CC; color: #ffffff; font-weight: bold; text-align: center; vertical-align: middle;border-right: 0px solid #ffffff;border-left: 0px solid #ffffff;}'; 
strFrame += '.ArrowU{  width:28px; cursor: hand; font-family: Arial; font-size: 11px; background-color: #6699CC; color: #ffffff; font-weight: bold; text-align: center; vertical-align: middle;border: 2px outset #ffffff;}'; 
strFrame += '.YearMonth{  cursor: hand;  font-family: Arial; font-size: 11px; background-color: #6699CC; color: #ffffff; font-weight: bold; text-align: center; vertical-align: middle;}'; 
strFrame += '.DateSelect{ cursor: hand;  font-family: Arial; font-size: 11px; background-color: #6699CC; color: #ffffff; font-weight: bold; text-align: center;}'; 
strFrame += '.TableCSS{ border: 1px solid #6699CC;border-collapse:collapse;border-spacing:0;width:220px;height:160px;background-color:#ffffff;}';
strFrame += '</style>'; 

strFrame += '<scr' + 'ipt>'; 
strFrame += 'var datelayerx,datelayery; /*存放日历控件的鼠标位置*/'; 
strFrame += 'var bDrag; /*标记是否开始拖动*/'; 
strFrame += 'document.onmousemove=document_mousemove;function document_mousemove() /*在鼠标移动事件中，如果开始拖动日历，则移动日历*/';
strFrame += '{ try{';
strFrame += '    if(bDrag && window.event.button==1)'; 
strFrame += '       {   var DateLayer=parent.document.all.myDateLayer.style;'; 
strFrame += '           DateLayer.posLeft += window.event.clientX-datelayerx;/*由于每次移动以后鼠标位置都恢复为初始的位置，因此写法与div中不同*/'; 
strFrame += '           DateLayer.posTop += window.event.clientY-datelayery;';
strFrame += '       }';
strFrame += '  } catch(e){}';
strFrame += '}'; 
strFrame += 'function DragStart() /*开始日历拖动*/'; 
strFrame += '{ try{';
strFrame += '    var DateLayer=parent.document.all.myDateLayer.style;'; 
strFrame += '    datelayerx=window.event.clientX;'; 
strFrame += '    datelayery=window.event.clientY;'; 
strFrame += '    bDrag=true;}';
strFrame += '  catch(e){}';
strFrame += '}'; 
strFrame += 'function DragEnd()';
strFrame += ' { /*结束日历拖动*/'; 
strFrame += '    bDrag=false;';
strFrame += '}'; 
strFrame += '</scr' + 'ipt>';
 
strFrame += '<div style="z-index:9999;position: absolute; left:0; top:0;" onselectstart="return false">';
strFrame += '<span id=tmpSelectYearLayer  style="z-index: 9999; position: absolute;top: 3; left: 30; display: none"></span>'; 
strFrame += '<span id=tmpSelectMonthLayer style="z-index: 9999; position: absolute;top: 3; left: 120;display: none"></span>'; 
strFrame += '<table class="TableCSS">'; 
strFrame += '<tr><td width=220px height=22px bgcolor=#6699cc>';
strFrame += '<table border=0 cellspacing=0 cellpadding=0 width=220 height=22 >'; 
strFrame += '<tr align=center >'; 

strFrame += '<td width=90px align=center style="font-size:11px;cursor:hand" class="YearMonth" onclick="parent.tmpSelectYearInnerHTML(this.innerText.substring(0,4))" ';
strFrame +=           ' title="Click here to select Year"><span id=nickYearHead></span></td>'; 
strFrame += '<td width=25 align=center  class="YearMonth" onclick="parent.nickPrevM()" title="Back to previous month" ><b >&lt;</b></td>';
strFrame += '<td width=76px  align=center style="font-size:12px;cursor:hand" class="YearMonth" onclick="parent.tmpSelectMonthInnerHTML(this.innerText.length==3?this.innerText.substring(0,1):this.innerText.substring(0,2))"  ';
strFrame +=           ' title="Click here to select Month">'; 
strFrame += '<span id=nickMonthHead ></span></td>'; 
strFrame += '<td width="25px" align=center class="YearMonth" onclick="parent.nickNextM()" title="Go forward to next month" ><b >&gt;</b></td></tr>'; 
strFrame += '</table></td></tr>'; 
strFrame += '<tr ><td width=220px height=18px >'; 
strFrame += '<table border=0 cellspacing=0 cellpadding=0 bgcolor=#AEC9E3 ' + (bMoveable? 'onmousedown="DragStart()" onmouseup="DragEnd()"':''); 
strFrame += ' BORDERCOLORLIGHT=#9496E1 BORDERCOLORDARK=#FFFFFF class="WeekDay" style="WIDTH: 220px;height:20px; cursor:' + (bMoveable ? 'move':'default') + '" >'; 
strFrame += '<tr align=center valign=bottom><td class="WeekEnd" >S</td>'; 
strFrame += '<td class="WeekDay" >M</td><td  class="WeekDay" >T</td>'; 
strFrame += '<td class="WeekDay" >W</td><td  class="WeekDay" >T</td>'; 
strFrame += '<td class="WeekDay" >F</td><td  class="WeekEnd" >S</td></tr>'; 
strFrame += '</table></td></tr>'; 
strFrame += '<tr><td width=220px height=120px>'; 


//strFrame += ' <table border=1 cellspacing=2 cellpadding=0 BORDERCOLORLIGHT=#9496E1 BORDERCOLORDARK=#FFFFFF bgcolor=#fff8ec width=220 height=120 >'; 
strFrame += '<table border=0 cellspacing=0 cellpadding=0 bgcolor=#FFFFFF width="220px" height="120px" >'; 

var n=0; for (var j=0;j<5;j++)
{
    strFrame +=  ' <tr align=center >'; 
    for (var i=0;i<7;i++)
     { 
        if(i==0 || i==6)
        {
           strFrame += '<td class="DayW" width="30px" height="20px" id=nickDay' + n + ' onclick=parent.nickDayClick(this.innerText,0)></td>'; 
        }
        else
        {    
           strFrame += '<td class="DayN" width="30px" height="20px" id=nickDay' + n + ' onclick=parent.nickDayClick(this.innerText,0)></td>'; 
        }
        n++;
     } 
    strFrame += '</tr>';
} 
strFrame += ' <tr align=center >'; 

for (var i=35;i<39;i++)
{
    strFrame += '<td class="DayN" width=30px height=20px id=nickDay' + i + ' style="font-size:11px" onclick="parent.nickDayClick(this.innerText,0)"></td>'; 
 }   
strFrame += '<td class="DayNC"  colspan=3 align=right>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;';
strFrame += '<span onclick=javascript:{try{parent.closeLayer();parent.outObject.value=\"\";}catch(e){}} style="font-size:12px;cursor: hand" '; 
strFrame += 'title="' + _VersionInfo + '"><u>Cancel</u></span>&nbsp;</td></tr>';
strFrame += '</table>'; 
strFrame += '</td></tr><tr ><td >'; 

strFrame += '<table border=0 cellspacing=0 cellpadding=0 width=100% bgcolor=#6699CC>'; 
strFrame += '<tr><td align=left>';
strFrame += '<input type=button class=button value="<<" title="Back to previous year" onclick="parent.nickPrevY()" onfocus="this.blur()" >'; 
strFrame += '<input class=button title="Back to previous month" type=button value="< " onclick="parent.nickPrevM()" onfocus="this.blur()" >';
strFrame += '</td>'; 
strFrame += ' <td align=center>';
strFrame += '<input type=button class=button value=Today onclick="parent.nickToday()" onfocus="this.blur()" title="Current date" style="font-size: 12px; width:42px;">';
strFrame += '</td>'; 
strFrame += ' <td align=right>';
strFrame += '<input type=button class=button value=" >" onclick="parent.nickNextM()" onfocus="this.blur()" title="Go forward to next month" >'; 
strFrame += '<input type=button class=button value=">>" title="Go forward to next year" onclick="parent.nickNextY()" onfocus="this.blur()" >';
strFrame += '</td></tr>'; 
strFrame += '</table></td></tr></table></div>'; 

//==Begin========================================================================================================== 
try
{
window.frames.myDateLayer.document.writeln(strFrame); 
window.frames.myDateLayer.document.close();  //解决ie进度条不结束的问题 

//==================================================== WEB 页面显示部分 =========================================== 
var outObject; 
var outButton;     //点击的按钮 
var outDate = "";  //存放对象的日期 
var odatelayer = window.frames.myDateLayer.document.all; //存放日历对象 

var wscDateFormat = "-";

//-----------------------------------------------------------------------------------------------------Begin
window.onerror = doError;
window.frames.myDateLayer.document.onmouseover = raiseCel;
window.frames.myDateLayer.document.onmouseout = sinkCel;
}
catch(e)
{}
//==End========================================================================================================== 


function doError() {
	//alert(arguments[0]);
}

function raiseCel() 
{    
	objItem = window.frames.myDateLayer.event.srcElement;
	if (objItem.className == "DayN")
		if (objItem.innerText != " " && objItem.innerText != "") 		   objItem.className = "DayNU";
		   
	if (objItem.className == "DayW")
		if (objItem.innerText != " " && objItem.innerText != "") 		   objItem.className = "DayWU";
		
    if (objItem.className == "ArrowN")         objItem.className = "ArrowU";
}

function sinkCel() 
{
	objItem = window.frames.myDateLayer.event.srcElement;
	if (objItem.className == "DayNU")    objItem.className = "DayN";
	if (objItem.className == "DayWU")    objItem.className = "DayW";
	if (objItem.className == "ArrowU")   objItem.className = "ArrowN";
}



//-----------------------------------------------------------------------------------------------------End
 
//主调函数 
//calendar(this,"")
function WSCCalendar(tt, obj, dtFormat, beforeYear, afterYear)  //Albee Add beforeYear,afterYear 2012.04.06
 {  
    if (arguments.length > 5)   {  alert("对不起！传入本控件的参数太多！");      return;  } 
    if (arguments.length == 0)  {  alert("对不起！您没有传回本控件任何参数！");  return;  } 
    
    //--------Added by Anson for date format -- Begin
    if (dtFormat==null || typeof(dtFormat)=="undefined" || typeof(dtFormat)=="[object]" || dtFormat=="undefined" || dtFormat=="[object]")
       {
          dtFormat = "/";
       }
       
    if (dtFormat!="/" && dtFormat!="-" && dtFormat!="")
       {
          return; 
          event.returnValue=false; 
       }
       
    wscDateFormat = dtFormat;
    //---------------------------------------- End
    if (arguments[3] != null)
    {
        WSC_Render_Calendar_Before_Year = new Number(arguments[3]);
    }
    if (arguments[4] != null)
    {
        WSC_Render_Calendar_After_Year = new Number(arguments[4]);
    }
    
    var dads = document.all.myDateLayer.style; 
    var th   = tt; 
    var ttop = tt.offsetTop;    //TT控件的定位点高 
    var thei = tt.clientHeight; //TT控件本身的高 
    var tleft = tt.offsetLeft;  //TT控件的定位点宽 
    var ttyp = tt.type;         //TT控件的类型 
    
    while (tt = tt.offsetParent)  {  ttop+=tt.offsetTop;  tleft+=tt.offsetLeft;  } 
    
    dads.top = (ttyp=="image") ? ttop+thei+ "px": ttop+thei+6+ "px"; 
    dads.left = tleft + 10+"px"; 
    outObject = (arguments.length == 1) ? th   : obj; 
    outButton = (arguments.length == 1) ? null : th; //设定外部点击的按钮 
    //根据当前输入框的日期显示日历的年月 
    var reg = /^(\d+)\/(\d{1,2})\/(\d{1,2})$/; 
    var r = outObject.value.match(reg); 
    if(r!=null)
     { 
        r[2] = r[2] - 1; 
        var d= new Date(r[1], r[2],r[3]); 
        if(d.getFullYear()==r[1] && d.getMonth()==r[2] && d.getDate()==r[3])
         { 
            outDate=d; //保存外部传入的日期 
         } 
        else outDate=""; 
        nickSetDay(r[1],r[2]+1); 
     } 
    else
     { 
        outDate=""; 
        nickSetDay(new Date().getFullYear(), new Date().getMonth() + 1); 
    } 
    dads.display = ''; 

    event.returnValue=false; 
 } 


var MonHead = new Array(12); //定义阳历中每个月的最大天数 
MonHead[0] = 31;  MonHead[1] = 28;  MonHead[2] = 31;  MonHead[3] = 30;  MonHead[4] = 31;   MonHead[5] = 30; 
MonHead[6] = 31;  MonHead[7] = 31;  MonHead[8] = 30;  MonHead[9] = 31;  MonHead[10] = 30;  MonHead[11] = 31; 

var nickTheYear  = new Date().getFullYear(); //定义年的变量的初始值 
var nickTheMonth = new Date().getMonth()+1;  //定义月的变量的初始值 
var nickWDay     = new Array(39);            //定义写日期的数组 


//任意点击时关闭该控件 //ie6的情况可以由下面的切换焦点处理代替
document.onclick = function()  
 { 
    with(window.event) 
     { 
       if (srcElement.getAttribute("Author")==null && srcElement != outObject && srcElement != outButton) 
          closeLayer(); 
     } 
 } 


//按Esc键关闭，切换焦点关闭
document.onkeyup = function()  
{ 
    if (window.event.keyCode==27)
     { 
        if(outObject) outObject.blur(); 
        closeLayer(); 
     } 
    else if(document.activeElement) 
        if(document.activeElement.getAttribute("Author")==null && document.activeElement != outObject && document.activeElement != outButton)         
          closeLayer(); 
         
} 


//往 head 中写入当前的年与月
function nickWriteHead(yy,mm)  
 { 
    odatelayer.nickYearHead.innerText  =  yy ; //+ " 年"; 
    odatelayer.nickMonthHead.innerText =  mm ; //+ " 月"; 
 } 
 

//年份的下拉框
function tmpSelectYearInnerHTML(strYear)  
{ 
    if (strYear.match(/\D/)!=null)  {  alert("年份输入参数不是数字！");          return;  } 
    
    var m = (strYear) ? strYear : new Date().getFullYear();     
    //if (m < 1977 || m > 2020)       {  alert("年份值不在 1000 到 9999 之间！");  return;  } 
    
    var n = new Date().getFullYear()-WSC_Render_Calendar_Before_Year;// 2005; //m - 1; //Albee new Date().getFullYear()-2; 2012.03.24
    //if (n < 2005)       n = 2005; 
    //if (n + 1> 2020)    n = 2020; 
    
    var s = "<select name=tmpSelectYear class='DateSelect' style='width:60px; font-size: 11px' " 
        s += "onblur='document.all.tmpSelectYearLayer.style.display=\"none\"' " 
        s += "onchange='document.all.tmpSelectYearLayer.style.display=\"none\";" 
        s += "parent.nickTheYear = this.value; parent.nickSetDay(parent.nickTheYear,parent.nickTheMonth)'>\r\n"; 
        
    var selectInnerHTML = s; 
    //n = n - 10; 
    
    for (var i = n; i < n + WSC_Render_Calendar_Before_Year + WSC_Render_Calendar_After_Year; i++) 
    { 
       if (i == m) 
          {  selectInnerHTML += "<option value='" + i + "' selected>" + i + "</option>\r\n";  } 
       else 
          {  selectInnerHTML += "<option value='" + i + "'>" + i + "</option>\r\n";  } 
    } 
    selectInnerHTML += "</select>"; 
    odatelayer.tmpSelectYearLayer.style.display=""; 
    odatelayer.tmpSelectYearLayer.innerHTML = selectInnerHTML; 
    odatelayer.tmpSelectYear.focus(); 
} 


//月份的下拉框
function tmpSelectMonthInnerHTML(strMonth)  
{ 
    if (strMonth.match(/\D/)!=null)  {  alert("月份输入参数不是数字！");  return;  } 
    var m = (strMonth) ? strMonth : new Date().getMonth() + 1; 
    var s = "<select name=tmpSelectMonth class='DateSelect' style='width:80px; font-size: 11px' " 
    s += "onblur='document.all.tmpSelectMonthLayer.style.display=\"none\"' " 
    s += "onchange='document.all.tmpSelectMonthLayer.style.display=\"none\";" 
    s += "parent.nickTheMonth = this.value; parent.nickSetDay(parent.nickTheYear,parent.nickTheMonth)'>\r\n"; 
    var selectInnerHTML = s; 
   
    for (var i = 1; i < 13; i++) 
    { 
        var Disp = "";        
        if ( i==1 )   Disp = "January";
        if ( i==2 )   Disp = "February";
        if ( i==3 )   Disp = "March";
        if ( i==4 )   Disp = "April";
        if ( i==5 )   Disp = "May";
        if ( i==6 )   Disp = "June";
        if ( i==7 )   Disp = "July";
        if ( i==8 )   Disp = "August";
        if ( i==9 )   Disp = "September";
        if ( i==10 )  Disp = "October";
        if ( i==11 )  Disp = "November";
        if ( i==12 )  Disp = "December";
        
        if (i == m) 
        {         
           selectInnerHTML += "<option value='"+i+"' selected>" + Disp + "</option>\r\n";
        } 
        else 
        {           
           selectInnerHTML += "<option value='"+i+"' >" + Disp + "</option>\r\n";
        } 
    } 
    selectInnerHTML += "</select>"; 
    
    odatelayer.tmpSelectMonthLayer.style.display = ""; 
    odatelayer.tmpSelectMonthLayer.innerHTML = selectInnerHTML;    
    
    odatelayer.tmpSelectMonth.focus(); 
} 

//关闭层
function closeLayer()  
{ 
   try
   {
      document.all.myDateLayer.style.display = "none"; 
   }
   catch(e){}   
} 

//判断是否闰平年
function IsPinYear(year)  
{ 
    if (0 == year % 4 && ((year % 100 != 0) || (year % 400 == 0)))  return true;
    else return false; 
} 

//闰年二月为29天 
function GetMonthCount(year,month) 
{ 
    var c=MonHead[month-1];
    if((month==2) && IsPinYear(year)) c++;
    return c; 
} 

//求某天的星期几
function GetDOW(day,month,year)  
{ 
    var dt = new Date(year,month-1,day).getDay()/7;    return dt; 
} 

//往前翻 Year 
function nickPrevY() 
{ 
    nickTheYear--;
    nickSetDay(nickTheYear,nickTheMonth); 
} 

//往后翻 Year
function nickNextY()  
{ 
    nickTheYear++; 
    nickSetDay(nickTheYear,nickTheMonth); 
} 

//Today Button 
function nickToday() 
{ 
    var today; 
    nickTheYear  = new Date().getFullYear(); 
    nickTheMonth = new Date().getMonth()+1; 
    if (nickTheMonth < 10)  {  nickTheMonth = "0" + nickTheMonth;  } 
    today=new Date().getDate(); 
    if (today < 10)  {  today = "0" + today;  } 
    //nickSetDay(nickTheYear,nickTheMonth); 
    if(outObject)
       outObject.value=nickTheYear + "/" + nickTheMonth + "/" + today; 

    closeLayer(); 
} 

//往前翻月份
function nickPrevM()  
{ 
    if(nickTheMonth>1)  {  nickTheMonth--;  }
    else  {  nickTheYear--;  nickTheMonth=12;    } 
    nickSetDay(nickTheYear,nickTheMonth); 
} 

//往后翻月份
function nickNextM()  
{ 
    if(nickTheMonth==12)  {  nickTheYear++;  nickTheMonth=1  }
    else  {  nickTheMonth++;  } 
    nickSetDay(nickTheYear,nickTheMonth); 
} 

//主要的写程序********** 
function nickSetDay(yy,mm) 
{ 
   try
   {
        nickWriteHead(yy,mm); 
        //设置当前年月的公共变量为传入值 
        nickTheYear=yy; 
        nickTheMonth=mm; 

        for (var i = 0; i < 39; i++){nickWDay[i]=""};                //将显示框的内容全部清空 
        var day1 = 1,day2=1,firstday = new Date(yy,mm-1,1).getDay(); //某月第一天的星期几 
        for (i=0;i<firstday;i++)  
              nickWDay[i]=GetMonthCount(mm==1 ? yy-1: yy,mm==1 ? 12:mm-1)-firstday + i + 1; //上个月的最后几天 
        for (i = firstday; day1 < GetMonthCount(yy,mm)+1; i++) 
              {  nickWDay[i]=day1;day1++; } 
        for (i=firstday+GetMonthCount(yy,mm);i<39;i++) 
              {  nickWDay[i]=day2;day2++  } 
        for (i = 0; i < 39; i++) 
        { 
            var da = eval("odatelayer.nickDay"+i)                //书写新的一个月的日期星期排列 
              
            if (nickWDay[i]!="") 
            { 
                //初始化边框 
                da.borderColorLight="#AEC9E3"; 
                da.borderColorDark="#FFFFFF"; 
                
                if(i<firstday)   //上个月的部分 
                { 
                    da.innerHTML="<b><font color=gray>" + nickWDay[i] + "</font></b>"; 
                    da.title= nickWDay[i] + "-" + (mm==1?12:mm-1) +"-" + yy; 
                    da.onclick=Function("nickDayClick(this.innerText,-1)"); 
                    if(!outDate) 
                        da.style.backgroundColor = ((mm==1?yy-1:yy) == new Date().getFullYear() && 
                        (mm==1?12:mm-1) == new Date().getMonth()+1 && nickWDay[i] == new Date().getDate()) ? "#C6C7EF":"#FFFFFF"; 
                else 
                { 
                    da.style.backgroundColor =((mm==1?yy-1:yy)==outDate.getFullYear() && (mm==1?12:mm-1)== outDate.getMonth() + 1 && 
                    nickWDay[i]==outDate.getDate())? "#FFD700" : 
                        (((mm==1?yy-1:yy) == new Date().getFullYear() && (mm==1?12:mm-1) == new Date().getMonth()+1 && nickWDay[i] == new Date().getDate()) 
                        ? "#C6C7EF":"#FFFFFF"); 
                    
                    ////将选中的日期显示为凹下去 
                    //if((mm==1?yy-1:yy)==outDate.getFullYear() && (mm==1?12:mm-1)== outDate.getMonth() + 1 && nickWDay[i]==outDate.getDate()) 
                    //{ 
                    //    da.borderColorLight="#FFFFFF"; 
                    //    da.borderColorDark="#AEC9E3"; 
                    //} 
                } 
            } 
            else if (i>=firstday+GetMonthCount(yy,mm))  //下个月的部分 
            { 
                da.innerHTML="<b><font color=gray>" + nickWDay[i] + "</font></b>"; 
                da.title= nickWDay[i] + "-" + (mm==12?1:mm+1) +"-" + yy; 
                da.onclick=Function("nickDayClick(this.innerText,1)"); 
                if(!outDate) 
                    da.style.backgroundColor = ((mm==12?yy+1:yy) == new Date().getFullYear() && 
                        (mm==12?1:mm+1) == new Date().getMonth()+1 && nickWDay[i] == new Date().getDate()) ? 
                        "#C6C7EF":"#FFFFFF"; 
                else 
                { 
                    da.style.backgroundColor =((mm==12?yy+1:yy)==outDate.getFullYear() && (mm==12?1:mm+1)== outDate.getMonth() + 1 && 
                    nickWDay[i]==outDate.getDate())? "#FFD700" : 
                        (((mm==12?yy+1:yy) == new Date().getFullYear() && (mm==12?1:mm+1) == new Date().getMonth()+1 && 
                        nickWDay[i] == new Date().getDate()) ? "#C6C7EF":"#FFFFFF"); 
                        
                    ////将选中的日期显示为凹下去 
                    //if((mm==12?yy+1:yy)==outDate.getFullYear() && (mm==12?1:mm+1)== outDate.getMonth() + 1 && 
                    //nickWDay[i]==outDate.getDate()) 
                    //{ 
                    //   da.borderColorLight="#FFFFFF"; 
                    //   da.borderColorDark="#9496E1"; 
                    //} 
                } 
            } 
            else //本月的部分 
            { 
                da.innerHTML="<b>" + nickWDay[i] + "</b>"; 
                da.title = nickWDay[i] + "-" + mm +"-" + yy; 
                da.onclick = Function("nickDayClick(this.innerText,0)");  //给td赋予onclick事件的处理 
                
                //如果是当前选择的日期，则显示紫色的背景；如果是当前日期，则显示亮紫色的背景 
                if(!outDate) 
                    da.style.backgroundColor = (yy == new Date().getFullYear() && mm == new Date().getMonth()+1 && nickWDay[i] == new Date().getDate())? 
                       "#C6C7EF":"#FFFFFF"; 
                else 
                {               
                    da.style.backgroundColor = (yy == outDate.getFullYear() && mm == outDate.getMonth() + 1 && nickWDay[i] == outDate.getDate())? 
                       "#6699CC":((yy == new Date().getFullYear() && mm == new Date().getMonth() + 1 && nickWDay[i] == new Date().getDate())? 
                       "#C6C7EF":"#FFFFFF");

                    //将选中的日期显示为凹下去 
                    if(yy==outDate.getFullYear() && mm== outDate.getMonth() + 1 && nickWDay[i]==outDate.getDate()) 
                    { 
                        da.borderColorLight="#C6C7EF"; 
                        da.borderColorDark="#9496E1"; 
                    }
                } 
            } 
            da.style.cursor="hand" 
        } 
        else 
        { 
          da.innerHTML = "";  da.style.backgroundColor="";  da.style.cursor="default"} 
        } 
   }
   catch(e){}     
} 


//点击显示框选取日期，主输入函数************* 
function nickDayClick(n,ex) 
{      
    var yy=nickTheYear; 
    var mm = parseInt(nickTheMonth)+ex; //ex表示偏移量，用于选择上个月份和下个月份的日期 
    //判断月份，并进行对应的处理 
    if(mm<1)
     { 
        yy--; 
        mm = 12 + mm; 
     } 
    else if(mm>12)
     { 
        yy++; 
        mm = mm - 12; 
     } 

    if (mm < 10)  {mm = "0" + mm;} 
    
    if (outObject) 
     { 
        if (!n) {//outObject.value=""; 
            return;} 
        if ( n < 10)  {n = "0" + n;} 
        
        outObject.value= yy + wscDateFormat + mm + wscDateFormat + n ; //注：在这里你可以输出改成你想要的格式
        
        /*
        if (wscDateFormat=="/") 
           outObject.value= yy + "/" + mm + "/" + n ; //注：在这里你可以输出改成你想要的格式
        else if (wscDateFormat=="-")
           outObject.value= yy + "-" + mm + "-" + n ; //注：在这里你可以输出改成你想要的格式
        else if (wscDateFormat=="")
           outObject.value= yy + "" + mm + "" + n ; //注：在这里你可以输出改成你想要的格式
        else
           outObject.value= yy + "/" + mm + "/" + n ; //注：在这里你可以输出改成你想要的格式   
        */
        
        closeLayer(); 
     } 
    else {closeLayer(); alert("您所要输出的控件对象并不存在！");} 
} 