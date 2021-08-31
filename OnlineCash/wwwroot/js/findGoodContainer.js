class findGoodContainer {
    constructor(parent, event) {
        parent.innerHTML = `
<div class="row">
    <div class="col-11">
        <input id="findGoods" class="form-control form-control-sm " />
        <ul class="list-group" style="position: absolute; z-index: 999">
        </ul>
    </div>
    <span class="col-1 btn btn-sm btn-primary">Добавить</span>
</div>
`;
        let inputFind = parent.querySelector("input");
        let goodDatalist = parent.querySelector("ul");
        let btnAdd = parent.querySelector("span");
        let selectedGood = null;
        inputFind.onkeydown = function (e) {
            let list = goodDatalist.querySelectorAll("li");
            let selectedLi = goodDatalist.querySelector(".active");
            if (e.keyCode == 13 && selectedLi != null)
                fetch("/api/goods/" + selectedLi.dataset.id)
                    .then(resp => resp.json())
                    .then(good => {
                        inputFind.value = good.name;
                        selectedGood = good;
                        list.forEach(li => li.remove());
                        return;
                    })
            for (var i = 0; i < list.length; i++) {
                if (list[i] == selectedLi) {
                    selectedLi.classList.remove("active");
                    var previous = i == 0 ? list[list.length-1] : list[i - 1];
                    if (e.keyCode == 38)
                        previous.classList.add("active");
                    var next = i == list.length - 1 ? list[0] : list[i + 1];
                    if (e.keyCode == 40) {
                        next.classList.add("active");
                    };
                    break;
                }
            }

        }
        inputFind.oninput = function () {
            goodDatalist.querySelectorAll("li").forEach(li => li.remove());
            let str = findGoods.value;
            let firstElement = true;
            if (str.length > 2)
                fetch("/api/goods/find/" + str)
                    .then(resp => resp.json())
                    .then(goods => {
                        goods.forEach(good => {
                            let li = document.createElement("li");
                            li.dataset.id = good.id;
                            if (firstElement) {
                                li.classList.add("active");
                                firstElement = false;
                            }
                            li.onclick = function () {
                                goodDatalist.querySelectorAll("li").forEach(li => li.classList.remove("active"));
                                li.classList.toggle("active");
                                findGoods.value = li.textContent;
                                goodDatalist.querySelectorAll("li").forEach(li => li.remove());
                                selectedGood = good;
                            }
                            li.classList.add("list-group-item");
                            li.textContent = good.name;
                            goodDatalist.append(li);
                        })
                    })
        }
        btnAdd.onclick = function () {
            if (selectedGood !== null) event(selectedGood);
        }
    }
}