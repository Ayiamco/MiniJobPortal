


//------------------- javascript to hide and display hamburger menu ----------------------
document.getElementsByClassName("nav-icon")[0].addEventListener("click",
    function () {
        if (document.getElementById("nav-on-switch").style.display == "none") {

            document.getElementById("nav-on-switch").style.display = "inline";
            document.getElementById("nav-off-switch").style.display = "none";
            document.getElementById("hidden-nav").style.display = "none"

        }

        else {

            document.getElementById("nav-on-switch").style.display = "none";
            document.getElementById("nav-off-switch").style.display = "inline"
            document.getElementById("hidden-nav").style.display = "flex"
        }
    })
//##########################################################################################

//--------------- Javascript forn home pageg back to top button--------------------

//display back to top tooltip
document.getElementById("btn-top").addEventListener("mouseleave", function () {
    document.getElementById("mytool-tip").style.display = "none";
})

//hide back to top tooltip
document.getElementById("btn-top").addEventListener("mouseover", function () {
    document.getElementById("mytool-tip").style.display = "inline-block";
})


//hide and show back to top button
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


var isInViewport = function (elem) {
    var bounding = elem.getBoundingClientRect();
    return (
        bounding.top >= 0 &&
        bounding.left >= 0 &&
        bounding.bottom <= (window.innerHeight || document.documentElement.clientHeight) &&
        bounding.right <= (window.innerWidth || document.documentElement.clientWidth)
    );
};
//#######################################################################################################

