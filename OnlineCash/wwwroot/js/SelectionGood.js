class SelectionGood {
    constructor(goods = undefined) {
        if (goods == undefined) {
            this._loadGoods().then(groups => this._init(groups));
        }
        else
            this._init(goods);
        
    }
    OpenDialog(){
        this.selectedGoods.querySelectorAll("tr").forEach(tr=>tr.remove());
        this.modal.show();
    }
    CloseDialogSetEvent(callback){
        this.closeDialogEvent=callback;
    }
    CloseDialog(){
        let goods=[];
        this.selectedGoods.querySelectorAll("tr").forEach(tr=>goods.push(tr.dataset.id));
        this.closeDialogEvent(goods);
    }

    _init(goods) {
        let div = document.createElement("div");
        div.className = "modal fade";
        div.setAttribute("tabindex", -1);
        div.setAttribute("aria-labelledby", "exampleModalLabel");
        div.setAttribute("aria-hidden", "true");
        div.innerHTML = `
        <div class="modal-dialog modal-xl">
        <div class="modal-content">
            <div class="modal-header">
                <h5 class="modal-title" id="exampleModalLabel">Modal title</h5>
                <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
            </div>
            <div class="modal-body" style="height: calc(100vh - 200px); ">
                <div style="display: grid; grid-template-rows: auto 1fr; height: 100%">
                    <div class="mb-2">
                        <input list="findGoods" class="form-control form-control-sm" placeholder="Поиск">
                        <datalist id="findGoods"></datalist>
                    </div>
                    <div style=" display: grid; grid-template-columns: 1.5fr 4fr;">
                        <div groups style="overflow-y: auto; height:  calc(100vh - 470px); display: block; padding: 2px">
                        </div>
                        <div style=" display: block; height: calc(100vh - 470px); overflow: auto">
                            <table class="table table-sm table-hover" style="display: block; width: 100%; height: 100%; table-layout: fixed">
                                <col style="width: calc(100vw - 200px)">
                                <col style="width: 100px">
                                <thead>
                                <tr>
                                    <th>Наименование</th>
                                    <th>Ед.</th>
                                </tr>
                                </thead>
                                <tbody goods >
                                </tbody>
                            </table>
                        </div>
                    </div>
                    <div>
                        <div class="row m-1">
                            <div class="col-12">
                                <span move class="btn btn-sm btn-secondary" style="margin-right: 15px">Перенести </span>
                                <span move-all class="btn btn-sm btn-secondary">Перенести все</span>
                            </div>
                        </div>
                        <div style=" display: block; height: 178px; overflow: auto">
                            <table class="table table-sm table-hover" style="display: block; width: 100%; height: 100%; table-layout: fixed">
                                <col style="width: 30px">
                                <col style="width: calc(100vw - 200px)">
                                <col style="width: 100px">
                                <thead>
                                <tr>
                                    <th></th>
                                    <th>Выбранные товары</th>
                                    <th>Ед.</th>
                                </tr>
                                </thead>
                                <tbody selectedGoods >
                                </tbody>
                            </table>
                        </div>
                    </div>
                </div>
            </div>
            <div class="modal-footer">
                <span type="button" class="btn btn-secondary" data-bs-dismiss="modal">Отмена</span>
                <span btnSave type="button" class="btn btn-primary">Добавить</span>
            </div>
        </div>
    </div>
        `;
        this.divModal = div;
        this.modal = new bootstrap.Modal(div);
        div.querySelector("span[btnSave]").addEventListener("click", () => {
            this.modal.hide();
            this.CloseDialog();
        });
        let ulGroups = div.querySelector("div[groups]");
        let tableGoods = div.querySelector("tbody[goods]");
        this.goods = tableGoods;
        let selectedGoods = div.querySelector("tbody[selectedGoods]");
        this.selectedGoods = selectedGoods;
        let move = (good) => {
            let flagAdded = false;
            selectedGoods.querySelectorAll("tr").forEach(tr => {
                if (tr.dataset.id == good.id)
                    flagAdded = true;
            });
            if (!flagAdded) {
                let tr = document.createElement("tr");
                tr.dataset.id = good.id;
                tr.innerHTML = `<td><i class="fa fa-trash"></i></td><td>${good.name}</td><td>${good.unit}</td>`;
                tr.querySelector("i").addEventListener("click", () => tr.remove());
                selectedGoods.append(tr);
            }
        };
        let callGoods = (group) => {
            //console.log(group);
            let goods = new Array();
            if (group.goods !== undefined) goods = group.goods;
            if (group.groups !== undefined)
                group.groups.forEach(group => {
                    goods = goods.concat(callGoods(group));
                });
            return goods;
        }
        function addInTableGood(good) {
            let tr = document.createElement("tr");
            tr.dataset.id = good.id;
            tr.dataset.name = good.name;
            tr.dataset.unit = good.unit;
            tr.innerHTML = `<td>${good.name}</td><td>${good.unit}</td>`;
            tr.addEventListener("dblclick", () => move(good));
            tr.addEventListener("click", () => tr.classList.toggle("bg-primary"));
            tableGoods.append(tr);
        }
        let callTrees = (groups, parent) => {
            let ul = document.createElement("ul");
            groups.forEach(group => {
                let li = document.createElement("li");
                li.textContent = group.name;
                li.dataset.id = group.id;
                ul.append(li);
                if (group.groups !== undefined && group.groups.length > 0)
                    callTrees(group.groups, li);
                li.addEventListener("click", (e) => {
                    e.preventDefault();
                    e.stopPropagation();
                    tableGoods.querySelectorAll("tr").forEach(tr => tr.remove());
                    callGoods(group).forEach(good => addInTableGood(good));
                });
            });
            parent.append(ul);
        }
        callTrees(goods, ulGroups);
        this.groups = new Tree(ulGroups.querySelector("ul"));
        let inpSearch = div.querySelector("input");
        inpSearch.addEventListener("keyup", () => {
            tableGoods.querySelectorAll("tr").forEach(tr => tr.remove());
            let goods1 = callGoods({ groups: goods });
            goods1.forEach(good => {
                if (good.name.toLowerCase().indexOf(inpSearch.value.toLowerCase()) > -1) addInTableGood(good);
            })
        });
        let goodDataList = div.querySelector("#findGoods");
        callGoods({ groups: goods }).forEach(good => {
            let option = document.createElement("option");
            option.textContent = good.name;
            option.addEventListener("click", () => {
                tableGoods.querySelectorAll("tr").forEach(tr => tr.remove());
                let goods1 = callGoods({ groups: goods });
                goods1.forEach(good => {
                    if (good.name.toLowerCase().indexOf(inpSearch.value.toLowerCase()) > -1) addInTableGood(good);
                })
            })
            goodDataList.append(option);
        })
        let btnMove = div.querySelector("span[move]");
        let btnMoveAll = div.querySelector("span[move-all]");
        btnMove.addEventListener("click", () => tableGoods.querySelectorAll("tr").forEach(tr => {
            if (tr.classList.contains("bg-primary"))
                move({ id: tr.dataset.id, name: tr.dataset.name, unit: tr.dataset.unit });
            tr.classList.remove("bg-primary");
        }));
        btnMoveAll.addEventListener("click", () => tableGoods.querySelectorAll("tr").forEach(tr => {
            move({ id: tr.dataset.id, name: tr.dataset.name, unit: tr.dataset.unit });
        }));
    }

    _loadGoods() {
        return new Promise((resolve, reject) => {
            fetch("/api/goodgroups")
                .then(resp => resp.json())
                .then(groups => {
                    let groups_result = [];
                    groups.forEach(group => {
                        let newgroup = { id: group.id, name: group.name, goods: [], groups: [] };
                        group.goods.forEach(good => {
                            if (good.supplierId !== null) {
                                let flagAdd = true;
                                newgroup.groups.forEach(group => {
                                    if (group.id == good.supplierId) {
                                        group.goods.push({ id: good.id, name: good.name, unit: good.unit });
                                        flagAdd = false;
                                    }
                                })
                                if (flagAdd)
                                    newgroup.groups.push({
                                        id: good.supplier.id,
                                        name: good.supplier.name,
                                        goods: [{ id: good.id, name: good.name, unit: good.unit }]
                                    });
                            }
                            newgroup.goods.push({ id: good.id, name: good.name, unit: good.unit });
                        });
                        groups_result.push(newgroup);
                    });
                    resolve(groups_result);
                });
        });
    }
}