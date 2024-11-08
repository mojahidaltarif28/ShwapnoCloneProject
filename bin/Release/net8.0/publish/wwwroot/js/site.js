
document.querySelectorAll(".check-out-btn").forEach(button => {
    button.addEventListener("click", () => {

        const container = button.closest(".hblp-element");
        const productName = container.querySelector(".hblp-name").innerText;
        const price = container.querySelector(".hblp-amt").innerText;
        const unit = container.querySelector(".hblp-unit").innerText;
       
            window.location.href = `AddNumber?productName=${encodeURIComponent(productName)}&price=${encodeURIComponent(price)}&unit=${encodeURIComponent(unit)}&url=${encodeURIComponent(window.location.pathname)}`;
    });
});
