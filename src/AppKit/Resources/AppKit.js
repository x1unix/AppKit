//System Frame Prototype
function Frame(fname){
	this.Name = fname;
	window[fname] = this.Name;
}
Frame.prototype.Location = "about:blank";
Frame.prototype.Name = "";
Frame.prototype.Title = "undefined";
Frame.prototype.Height = 640;
Frame.prototype.Width = 640;
Frame.prototype.MaximiseBox = true;
Frame.prototype.MinimiseBox = true;
Frame.prototype.ControlBox = true;
Frame.prototype.Opacity = 100;
Frame.prototype.ShowInTaskbar = true;
Frame.prototype.Load = function(stdout){
	if(this.Name != "undefined"){
		if(this.Title == "undefined"){this.Title = this.Name;}
		var arg = [];
		arg["src"] = this.Location;
		arg["name"] = this.Name.toString();
		arg["title"] = this.Title.toString();
		arg["height"] = this.Height.toString();
		arg["width"] = this.Width.toString();
		arg["maxbox"] = this.MaximiseBox.toString();
		arg["minbox"] = this.MinimiseBox.toString();
		arg["control"] = this.ControlBox.toString();
		arg["opacity"] = this.Opacity.toString();
		arg["showintaskbar"] = this.ShowInTaskbar.toString();
		Framework.Query("frm_LoadFrame",arg,stdout);
	}else{
		eval("var "+stdout+" = 'Undefined name';");
	}
}
Frame.prototype.Show = function(stdout){
	if(this.Name != "undefined"){
		var arg = [];
		arg["name"] = this.Name.toString();
		Framework.Query("frm_ShowFrame",arg,stdout);
	}else{
		eval("var "+stdout+" = 'Undefined name';");
	}
}
Frame.prototype.Hide = function(stdout){
	if(this.Name != "undefined"){
		var arg = [];
		arg["name"] = this.Name.toString();
		Framework.Query("frm_HideFrame",arg,stdout);
	}else{
		eval("var "+stdout+" = 'Undefined name';");
	}
}
Frame.prototype.Close = function(stdout){
	if(this.Name != "undefined"){
		var arg = [];
		arg["name"] = this.Name.toString();
		Framework.Query("frm_CloseFrame",arg,stdout);
	}else{
		eval("var "+stdout+" = 'Undefined name';");
	}
}
Frame.FindAnd = function(){
	
}
Frame.FindAnd.Destroy = function (fname,stdout){
var arg = [];
arg["name"] = fname.toString();
Framework.Query("frm_CloseFrame",arg,stdout);
}
Frame.FindAnd.Show = function (fname,stdout){
var arg = [];
arg["name"] = fname.toString();
Framework.Query("frm_ShowFrame",arg,stdout);
}
Frame.FindAnd.Hide = function (fname,stdout){
var arg = [];
arg["name"] = fname.toString();
Framework.Query("frm_HideFrame",arg,stdout);
}





var Intent = function(intent_filter){
	Intent.prototype.__function = intent_filter;
	Intent.prototype.__data_assigned = false;
	Intent.prototype.__output = "undefined";
}
Intent.prototype.assignData = function(intent_data){
	Intent.prototype.__data_assigned = true;
	Intent.prototype.__intent_args = intent_data;
}
Intent.prototype.setOutput = function(intent_target){
	Intent.prototype.__output = intent_target;
}
Intent.prototype.call = function(){
	//build args for intent
	var x_out = "";	
	if(Intent.prototype.__data_assigned == true){
		var args = Intent.prototype.__intent_args;
		for (var key in args) { 
		var val = args [key];
		x_out = x_out + key+"="+val+"&";
		}
	}
	document.title = "intent://"+Intent.prototype.__function+"?"+x_out+"stdout="+Intent.prototype.__output;
};

//System Class Prototype
function Framework(){
}
//Environment query void
Framework.void = function(q_function, out_var){
	document.title = "intent://"+q_function+"?stdout="+out_var;
}
Framework._voidTimeout = function(){return true;}
Framework.Query = function(q_function, args, out_var){
var new_intent = new Intent(q_function);
new_intent.assignData(args);
new_intent.setOutput(out_var);
new_intent.call();
};

Framework.Environment = function(){
}

Framework.Environment.ExpandEnvironmentVariables = function(var_sequence, stdout){
	var arg = [];
	arg["var"] = var_sequence;
	Framework.Query("env_ExpandVariables",arg,stdout);
}

Framework.Shell = function(command){
	var srun = new Intent("CmdShell");
	var arg = [];
	arg["command"] = command;
	srun.assignData(arg);
	//srun.setOutput(stdout);
	srun.call();
}
Framework.SplashScreen = function(){var srun = new Intent("fr_Splash");var arg = [];srun.assignData(arg);srun.call();}

//Open File Dialog
function OpenFileDialog(){
}
OpenFileDialog.prototype.Title = "undefined";
OpenFileDialog.prototype.Filter = "undefined";
OpenFileDialog.prototype.ShowDialog = function(stdout){
var arg = [];
arg["title"] = this.Title;
arg["filter"] = this.Filter;
Framework.Query("mod_OpenFileDialog",arg,stdout);
};


//Messagebox
function MessageBox(content,title,icon,stdout){
	var arg = [];
	arg["content"] = content;
	arg["title"] = title;
	arg["icon"] = icon;
	Framework.Query("mod_MessageBox",arg,stdout);	
}
