//----------------javascript for search and filter params on the admins applicants filter-----------
console.log("got here")
document.getElementById("search").addEventListener("input", function (e) {
    document.getElementById("filterSearchParam").value = document.getElementById("search").value
})

document.getElementById("filter").addEventListener("input", function (e) {
    document.getElementById("searchFilterParam").value = document.getElementById("filter").value
})