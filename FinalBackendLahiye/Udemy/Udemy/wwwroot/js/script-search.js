const search = document.querySelector("#search");
const listItems = document.querySelectorAll(".subctg-search");
search.addEventListener("keyup", (e) => {
    const searchValue = e.target.value;
    listItems.forEach((e) => {
        console.log(e)
        if (e.textContent.toLowerCase().trim().includes(searchValue.toLowerCase().trim())) {
            
            e.style.display = "block";
        }
        else {
            e.style.display = "none";
        }
    })
})
