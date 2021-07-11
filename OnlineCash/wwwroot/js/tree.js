class Tree {
    constructor(tree, event=null) {
        tree.classList.add("tree");
        this.CallParent(tree, true);
        tree.querySelectorAll(".branch").forEach(branch=>{
            branch.querySelectorAll("li").forEach(li=>li.style.display="none");
            branch.addEventListener("click",(event)=>{
                event.stopPropagation();
                let i=branch.querySelector("i");
                let a=branch.querySelector("a");
                let ul=branch.querySelector("ul");
                [i].forEach(p=>{
                    p.addEventListener("click",()=>{
                        [].slice.call(ul.children).forEach(el=>{
                            if(el.nodeName.toLowerCase()=="li")
                                if(i.classList.contains("fa-folder"))
                                    el.style.display="list-item";
                                else
                                    el.style.display="none";
                        });
                        i.classList.toggle("fa-folder");
                        i.classList.toggle("fa-folder-open");
                    })
                })

                if(branch.classList.contains("fa-folder"))
                    [].slice.call(branch.children).forEach(ch=>{
                        if(ch.nodeName!="I" & ch.nodeName!="A"){
                            ch.style.display="list-item";
                        }
                    });
            })
        });
        if(event!=null)
            tree.querySelectorAll("li:not(.branch)").forEach(li=>li.addEventListener("click",()=>event(li)));
    }
    CallParent(ul, flagParent){
        let childLi=[].slice.call(ul.childNodes).filter(c=>c.nodeName.toLowerCase()=="li");
        if(childLi.length>0)
            childLi.forEach(li=>{
                if(li.querySelector("ul")!==null)
                {
                    li.classList.add("branch");
                    let i=document.createElement("i");
                    i.className="indicator fa fa-folder";
                    li.prepend(i);
                    li.querySelectorAll("ul").forEach(ul=>this.CallParent(ul, false));
                }
                if(flagParent==false)
                    li.style.display="none";
                else
                    li.style.display="list-item";
            });

    }
}