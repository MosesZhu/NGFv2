 /*<!--Callback 开始-->*/
 /******************************************************************************
 The following functions are presented to Anthem components
 Author: Anson Lin
 Date:   12-April-2006
 *******************************************************************************/
  
  
  /*Presents the confirming dialog window to user when user wanna delete the selected items 
    in the data list control, which contained the check options.*/
  function ConfirmDelHasChk_PreCallBack(button) 
      { 		  
        if(HasChecked()) 
        {
	 	  if (!confirm('Are you sure to delete the selected item(s)?')) 
		  {
		 	 return false;
		  }
		  
		  ShowLoading("");
		  
		  /*To disable the relevant controls*/			  	
		  /*
		  if(button!="" && button!=null && button!="undefined" && button!="[object]")
		  {
		    
		    var strArr = button.split("|s|");
		    try
		    {
		       var i = 0;
		       for(i=0;i<strArr.length;i++)
		       {
		          document.getElementById(strArr[i]).disabled = true;
		       }
		    }
		    catch(e)
		    {
		    
		    }   
		  }
		  */
		  
		} 
	  }
	  
  /*Presents the confirming dialog window to user when user wanna delete the selected items 
    in the data list control.*/
  function ConfirmDel_PreCallBack(button) 
    {        
 	      if (!confirm('Are you sure to delete the selected item(s)?')) 
	      {
	 	     return false;
	      }		
	      
	      ShowLoading(button);
	      /*To disable the relevant controls*/
	 } 
	
	
  /*Presents the confirming dialog window to user when user wanna delete the selected items 
    in the data list control, which contained the check options.*/
  function ConfirmUpdateHasChk_PreCallBack() 
      {     
		  
        if(HasChecked()) 
        {          
	 	  if (!confirm('Are you sure to update the selected item(s)?')) 
		  {
		 	 return false;
		  }
		  
		  ShowLoading("");
	  
		} 
	  }
	  		 
  /*Presents the confirming dialog window to user when user wanna delete the selected items 
    in the data list control.*/
  function ConfirmUpdate_PreCallBack(button) 
    {        
 	      if (!confirm('Are you sure to update the selected item(s)?')) 
	      {
	 	     return false;
	      }		
	      
	      ShowLoading(button);
	      /*To disable the relevant controls*/
	 } 

  /*Event after CallBack*/
  function btn_PostCallBack()
  {
	     RemoveLoading();
  }  

  function RemoveLoading()
  {
     try{
		   var loading = document.getElementById("div_loading");
	       document.body.removeChild(loading);
		}
	 catch(e){}	
  }
  
  function ShowLoading(msg)
	 {
	    try
	     {	
	        /*Show the DIV message to user*/            
		    var loading = document.createElement("div");
			loading.id = "div_loading";			
			//loading.className = "DIVGrid";
			loading.style.fontSize = "10pt";
            loading.style.height = "16px";            
            loading.style.borderWidth = "1px";
            loading.style.border = "1px";
            loading.style.borderColor = "DimGray";
            loading.style.borderStyle = "solid";
            loading.style.color = "Black";
            loading.style.backgroundColor = "#ffffcc";
            loading.style.paddingLeft = "8px";
			loading.style.paddingRight = "5px";
			loading.style.position = "absolute";						
			loading.style.zIndex = "9999";      
			loading.style.filter = "progid:DXImageTransform.Microsoft.Shadow(color='gray', Direction=135, Strength=2)";
			loading.style.fontSize = "9pt";   
			loading.style.fontFamily="Times New Roman,Arial, Helvetica, sans-serif;";
						
			var tempS = "Working...";
			
			if(msg=="" || msg==null || typeof(msg)=="undefined" || typeof(msg)=="[object]" || msg=="undefined" || msg=="[object]")  
			   tempS = "Working...";
			else
			   tempS = msg;
	        
	        //var imageUrl = window.location.protocol + "//" + window.location.host + "/NPM/SysFrame/images/loading_2.gif";
						
	        //var wMsg = "<table border=0 cellpadding=4 cellspacing=1 ><tr><td><img src='" + imageUrl + "' alt=''/></td><td><span>" + tempS + "</span></td></tr></table>";
	        
	        //loading.innerHTML = wMsg;
	        loading.innerHTML = tempS;
	           
	        loading.style.right = loading.style.width + 20 + "px";
	        //loading.style.left = Math.abs(document.body.getBoundingClientRect().right-loading.style.width) - 20;
			//loading.style.left = (Math.abs(document.body.getBoundingClientRect().left));// + "20px";
			loading.style.top = Math.abs(document.body.getBoundingClientRect().top) + "px"; //+ "20px";
			     
			document.body.appendChild(loading);			
		 }
		catch(e){} 	
	 }
	 	  
/*************************************
 Anthem components Supporting programs
 End
 *************************************/	  
 
 
  /*网格中是否有项被选中*/
 function HasChecked()
  {	         
      //var intCount=0;
      
	  for(var intCount=0;intCount< document.getElementsByTagName("input").length;intCount++)
      {			   
	    if (document.getElementsByTagName("input")[intCount].type=="checkbox")
	    {				    
		    if (document.getElementsByTagName("input")[intCount].id!="ChkAll" && document.getElementsByTagName("input")[intCount].disabled==false)
		    {
			    if(document.getElementsByTagName("input")[intCount].checked==true)						
			       return true;
		    }
	    }
      }
	  
      return false;
   }     	    
   

/*<!--Callback 结束-->*/




/*======================================================================================================*/
/*<!--公共方法 开始-->*/

var wsc_CurrentColor="";
       			
function wscChangeColor(obj,eColor)        /*改变颜色  */
{  
    try
    {        
        if(eColor=="")
           eColor = "PaleGoldenrod";
    	
	    wsc_CurrentColor=obj.style.backgroundColor;
        obj.style.backgroundColor="PaleGoldenrod";
        obj.style.cursor="pointer";
	}
	catch(ex){  }
}

         
function wscRestoreColor(obj)			/*恢复颜色  */
{   
    try{          
	    obj.style.backgroundColor=wsc_CurrentColor;	    
	}
	catch(ex){  }
}       

/*键盘松开时将小写转为大写，参数为控件ID，Anson Lin on 2004/11/19  */
function wscKeyUp_ToUpper(obj)
{
   try{      
       if(event.keyCode!=37 && event.keyCode!=39 && event.keyCode!=8 && event.keyCode!=9)
          {
            obj.value=obj.value.toUpperCase();  
          } 
   }          
   catch(ex){  }
}

/*键盘按下时剔除非字母和数字， 2004/11/19  */
function wscKeyDown_OnlyLetterAndNum()
{ 
   try{
        //alert(event.keyCode);
        //只能输入数字和字母                  
        if(!(
             (event.keyCode>=48 && event.keyCode<=57)  ||
              event.keyCode==32 || event.keyCode==8 || event.keyCode==9 || event.keyCode==46 ||
              event.keyCode==37 || event.keyCode==39 || event.keyCode==190 ||
             (event.keyCode>=97 && event.keyCode<=122) ||
             (event.keyCode>=65 && event.keyCode<=90)
             ))
            event.returnValue=false;
   }         
   catch(ex){  }
}			

/*键盘按下时剔除非数字， 2004/11/19 */
function wscKeyDown_OnlyNum()
{
   try{
        //只能输入数字   
     if(event.keyCode!=8)
        event.returnValue = ( (/\d/.test(String.fromCharCode(event.keyCode))) || 
                              (/\t/.test(String.fromCharCode(event.keyCode)))
                             );                               
   }
   catch(ex){  }
}	

/*键盘按下时剔除非字母， on 2004/11/19  */
function wscKeyDown_OnlyLetter()
{
   try{
        /*只能输入字母     */
     if(event.keyCode!=8)                       
        event.returnValue = (/[a-zA-Z]/.test(String.fromCharCode(event.keyCode)));    
   }
   catch(ex){  }     
}	
/*<!--公共方法 结束-->*/