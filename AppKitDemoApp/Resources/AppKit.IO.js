var Directory = function(){};
Directory.Exists = function(dir,out){
	var arg = [];
	arg["path"] = dir;
	Framework.Query("fm_DirExists",arg,out);
};
Directory.Create = function(dir,out){
	var arg = [];
	arg["path"] = dir;
	Framework.Query("fm_DirCreate",arg,out);
};
Directory.Delete = function(dir,out){
	var arg = [];
	arg["path"] = dir;
	Framework.Query("fm_DirDelete",arg,out);
};
Directory.GetFiles= function (dir,out){
	var arg = [];
	arg["path"] = dir;
	Framework.Query("fm_GetFiles",arg,out);
}
Directory.GetDirectories= function (dir,out){
	var arg = [];
	arg["path"] = dir;
	Framework.Query("fm_GetDirectories",arg,out);
}
Directory.Root= function (out){
	var i = new Intent("fm_GetDirectoriesRoot");
	i.setOutput(out);
	i.call();
}


//File manage
function File(){
}
File.Exists = function(filename, stdout){
var arg = [];
arg["filename"] = filename;
Framework.Query("fm_FileExists",arg,stdout);	
};
File.Delete = function(filename, stdout){
var arg = [];
arg["filename"] = filename;
Framework.Query("fm_DeleteFile",arg,stdout);	
};
File.Move = function(filename,destination, stdout){
var arg = [];
arg["filename"] = filename;
arg["destination"] = destination;
Framework.Query("fm_MoveFile",arg,stdout);	
};
File.Copy = function(filename,destination, stdout){
var arg = [];
arg["filename"] = filename;
arg["destination"] = destination;
Framework.Query("fm_CopyFile",arg,stdout);	
};
File.Read = function(filename,stdout){
var arg = [];
arg["filename"] = filename;
Framework.Query("fm_ReadFile",arg,stdout);	
};





