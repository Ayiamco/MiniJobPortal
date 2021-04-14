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



////animation using gsap
//gsap.registerPlugin(ScrollTrigger)
////nav-bar animation
//gsap.from("#top", { duration: 1, ease: "bounce", y: "-200%" })
//gsap.from(".nav-link", { duration: 1, delay: 1, opacity: 0, stagger: .2 })

////main section animation
//gsap.from('.main-bd', { duration: 1, ease: "power2", x: "-200%", delay: 2 })
//gsap.from('.main-hero', { duration: 1, ease: "bounce", x: "200%", delay: 2 })

////services section animation 
//gsap.from('.services-container', {
//    duration: 1, ease: 'linear', y: "20%", scrollTrigger: {
//        trigger: ".services-container",
//        toggleActions: "restart none restart none"
//    }
//}
//)

////approach  section animations
//gsap.from('#approach-img', {
//    duration: 1, delay: 2, ease: "linear", x: "100%", scrollTrigger: {
//        trigger: "#approach-img",
//        toggleActions: "restart none restart none"
//    }
//}
//)
//gsap.from('#approach-bd', {
//    duration: 1, delay: 2, ease: "linear", x: "-100%", scrollTrigger: {
//        trigger: "#approach-bd",
//        toggleActions: "restart none restart none"
//    }
//}
//)
