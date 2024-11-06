import React, { useEffect, useRef, useState } from "react";
import LanguageOption from "../components/options/LanguageOption";
import { useTranslation } from "react-i18next";
import style from "../GlobalStyle.module.css"
function MenuSection() {
    const { t } = useTranslation(["menu"]);
    const tags = t('menu:menu', { returnObjects: true }) as { title: string, href: string }[];
    const [selectIndex, setSelectIndex] = useState<number>(0);
    return (
        <div className="flex flex-col md:pt-3 md:flex-row md:items-center justify-between h-[100%]">
            <div className="flex flex-col md:flex-row">
                {tags.map((tag, index) => <MenuTag key={index} title={tag.title} href={tag.href} index={index} isSelected={index === selectIndex}
                    handleClick={()=>setSelectIndex(index)} />)}
            </div>
            <div className="text-right p-1 pb-32 text-h3 text-menu-font">{"▹"}</div>
            <div className="p-5">
                <LanguageOption/>
            </div>
            
        </div>
    );
}
 
type MenuTagProps = {
    href: string;
    title: string;
    isSelected: boolean;
    index: number;
    handleClick: ()=>void;
}
function MenuTag(props:MenuTagProps) { 
    const aRef = useRef<HTMLAnchorElement>(null);
    return <a href={props.href} ref={aRef} onClick={props.handleClick}
                className={`p-5 ${props.isSelected? style.menuSelected : ""}  ${props.isSelected? "text-advanced font-bold": "text-basic" } hover:font-bold hover:text-menu-font text-h5 md:text-h4`}>
                {props.title}
            </a>
}
export default MenuSection;
