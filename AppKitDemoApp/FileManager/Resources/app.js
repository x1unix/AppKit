
var items = [];

function load(){
	Directory.Root("items");

	var container = i("app-container");
	//Directory.Root("items");
	//setTimeout(function(){for(i = 0; i < items.length; ++i){alert("Found Drive "+items[i]);}},250);
	for(i = 0; i < items.length; ++i){
		addItem(items[i],0);
		
		//window.title(items[key]);
	}

	//alert(items.length);
	//var e = items;
	//alert(e.length);
	//for(var key in e){
	//addItem(items[key],0);
	//alert(items[key]);
	//}
	//addItem("Folder",0);
	//addItem("File",1);
}

function updateItems(){
	//clearItems();
	for(i = 0; i < items.length; ++i){
		addItem(items[i],0);
		alert(items[i]);
	}
}

function i(id){return document.getElementById(id);}
function addItem(name,objID){
	var container = i("app-container");
	var item = document.createElement("div");
	item.className = "grid-item";
	var insert = ["folder.png","file.png"];
	if(objID > 0){
	item.setAttribute("onclick","openFile('"+name+"');");
	}else{
		item.setAttribute("onclick","openDir('"+name+"');");
	}

	item.innerHTML = "<img src=\""+insert[objID]+"\"/>\n<span>"+name+"</span>\n";

	container.appendChild(item);
}
function clearItems(){
	var container = i("app-container");
	container.innerHTML = "";
}
function openDir(path){
	alert(path);
}
function openFile(path){
alert("This is File");
}