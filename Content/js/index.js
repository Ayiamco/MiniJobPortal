//function to check if element is in view port
var isInViewport = function (elem) {
    var bounding = elem.getBoundingClientRect();
    return (
        bounding.top >= 0 &&
        bounding.left >= 0 &&
        bounding.bottom <= (window.innerHeight || document.documentElement.clientHeight) &&
        bounding.right <= (window.innerWidth || document.documentElement.clientWidth)
    );
};


//Event listener to show nav bar when button is clicked
document.getElementById("hide-nav").addEventListener("click", () => {
    document.getElementById("hide-nav").style.display = "none";
    document.getElementById("show-nav").style.display = "block";
    document.getElementsByTagName("nav")[0].style.display = "none";

})

//Event listener to hide nav bar when button is clicked
document.getElementById("show-nav").addEventListener("click", () => {
    document.getElementById("hide-nav").style.display = "block";
    document.getElementById("show-nav").style.display = "none";
    document.getElementsByTagName("nav")[0].style.display = "block";

})

//Event listener to display scroll back to top icon
document.addEventListener("scroll", function () {
    var element = document.getElementById("top")
    if (isInViewport(element)) {
        document.getElementById("btn-top").style.display = "none"
    }
    else {
        document.getElementById("btn-top").style.display = "block"
    }
})
window.onload = function () {
    document.getElementById("btn-top").style.display = "none"
}



window.addEventListener("resize", () => {
    console.log("resized")
})
