
var items = [];

function i(id){return document.getElementById(id);}
function addItem(objct, asDrive){
	var objID = 0;
	if(objct.Type == "File"){objID = 1;}
	if(asDrive=="true"){objID = 2;}
	var item = document.createElement("div");
	item.className = "grid-item";
	var insert = ["folder.png","file.png","drive.png"];
	switch(objID){
		case 0:
		item.setAttribute("onclick","goto_dir('"+objct.Path+"');");
		break;
		case 1:
		item.setAttribute("onclick","openFile('"+objct.Path+"');");
		break;
		default:
		item.setAttribute("onclick","goto_dir('"+objct.Path+"');");

	}
	item.innerHTML = "<img src=\""+insert[objID]+"\"/>\n<span>"+objct.Name+"</span>\n";
	container.appendChild(item);
}
function openFile(path){
Framework.Shell(path);
}