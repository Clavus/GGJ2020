(categorySelect = function() {

    var categoryCount = 4;
    var categoryArray = [];

    function getRandomInt(min, max) {
        min = Math.ceil(min);
        max = Math.floor(max);
        return Math.floor(Math.random() * (max - min + 1)) + min;
    }

    ($.getJSON( "../data/data.json", function( data ) {

        for(var i = 0; i < categoryCount; i++){
            var randomNumber = getRandomInt(0, data.Categories.length - 1);
            if(categoryArray.length > 0){
                for(var y = 0; y < categoryArray.length; y++){
                    if(randomNumber == categoryArray[y]){
                        y = -1;
                        randomNumber = getRandomInt(0, data.Categories.length - 1);
                        continue;
                    }
                }
            }
            categoryArray.push(randomNumber);
        }

        document.getElementsByClassName('lefttop')[0].innerHTML = data.Categories[categoryArray[0]].name;
        document.getElementsByClassName('lefttop')[0].id = categoryArray[0];
        document.getElementsByClassName('righttop')[0].innerHTML = data.Categories[categoryArray[1]].name;
        document.getElementsByClassName('righttop')[0].id = categoryArray[1];
        document.getElementsByClassName('leftbottom')[0].innerHTML = data.Categories[categoryArray[2]].name;
        document.getElementsByClassName('leftbottom')[0].id = categoryArray[2];
        document.getElementsByClassName('rightbottom')[0].innerHTML = data.Categories[categoryArray[3]].name;
        document.getElementsByClassName('rightbottom')[0].id = categoryArray[3]; 
    }));
})();

imageView = function(id) {
    switchView();
    imageArray = [];
    ($.getJSON("../data/data.json", function(data){
        for(var i = 0; i < data.Categories[id].imageCount; i++){
            imageArray.push(data.Categories[id].images[i].url);
        }
        
        imageArray = shuffle(imageArray);
        console.log(imageArray);
    
        for(var j = 0; j < imageArray.length; j++){
            console.log(imageArray[j]);
            appendImages(imageArray[j], "images");
        }
        imageArray = [];

    }))
};

appendImages = function(url, appendId){
    $("<img class='image-item' src=../images/"+ url +">").appendTo( "#" + appendId );
}

clearImages = function(id) {
    $("#" + id).empty();
}

switchView = function() {
    var x = document.getElementById("root");
    var y = document.getElementById("images");
    var bb = document.getElementById("backbutton");
    
    if (x.style.display === "none" && y.style.display === "grid" && bb.style.display == "block") {
        x.style.display = "grid";
        y.style.display = "none";
        bb.style.display = "none";
        clearImages("images");
    } else {
        x.style.display = "none";
        y.style.display = "grid";
        bb.style.display = "block";
    }
}

function shuffle(a) {
    var j, x, i;
    for (i = a.length - 1; i > 0; i--) {
        j = Math.floor(Math.random() * (i + 1));
        x = a[i];
        a[i] = a[j];
        a[j] = x;
    }
    return a;
}

