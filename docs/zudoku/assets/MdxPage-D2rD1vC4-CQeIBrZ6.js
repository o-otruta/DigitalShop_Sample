import{c as O,ad as R,ae as E,af as L,O as e,r as x,U as P,W as u,h as S,i as v,ag as T,ah as _,ai as y,p as q,aj as W,ak as z}from"./entry.client-DWncdgUP.js";/**
 * @license lucide-react v0.475.0 - ISC
 *
 * This source code is licensed under the ISC license.
 * See the LICENSE file in the root directory of this source tree.
 */const B=[["path",{d:"M21 12h-8",key:"1bmf0i"}],["path",{d:"M21 6H8",key:"1pqkrb"}],["path",{d:"M21 18h-8",key:"1tm79t"}],["path",{d:"M3 6v4c0 1.1.9 2 2 2h3",key:"1ywdgy"}],["path",{d:"M3 10v6c0 1.1.9 2 2 2h3",key:"2wc746"}]],I=O("ListTree",B),N="data-active",b=({item:t,children:s,className:a,isActive:r})=>e.jsxs("li",{className:u("truncate",r?"text-primary":"text-foreground/65 dark:text-foreground/75",a),title:t.value,children:[e.jsx(z,{to:`#${t.id}`,[N]:t.id,className:u(r?"text-primary":"text-foreground/65 dark:text-foreground/75 hover:text-foreground"),children:t.value}),s]}),U=({entries:t})=>{const{activeAnchor:s}=W(),a=x.useRef(null),r=x.useRef(!1),[d,n]=x.useState({top:0,opacity:0});return x.useEffect(()=>{if(!a.current)return;const i=a.current.querySelector(`[${N}='${s}']`);if(!i){n({top:0,opacity:0});return}const l=a.current.getBoundingClientRect().top,h=i.getBoundingClientRect().top;n({opacity:1,top:`${h-l}px`}),!r.current&&requestIdleCallback(()=>{r.current=!0})},[s]),e.jsxs("aside",{className:"sticky scrollbar top-[--header-height] h-[calc(100vh-var(--header-height))] pt-[--padding-content-top] pb-[--padding-content-bottom] overflow-y-auto ps-1 text-sm",children:[e.jsxs("div",{className:"flex items-center gap-2 font-medium mb-2",children:[e.jsx(I,{size:16}),"On this page"]}),e.jsxs("div",{className:"relative ms-2 ps-4",children:[e.jsx("div",{className:"absolute inset-0 right-auto bg-border w-[2px]"}),e.jsx("div",{className:u("absolute -left-px -translate-y-1 h-6 w-[4px] rounded bg-primary",r.current&&"ease-out [transition:top_150ms,opacity_325ms]"),style:d}),e.jsx("ul",{ref:a,className:"relative font-medium list-none space-y-2",children:t.map(i=>e.jsx(b,{isActive:i.id===s,item:i,className:"pl-0",children:i.children&&e.jsx("ul",{className:"list-none pl-4 pt-2 space-y-2",children:i.children.map(l=>e.jsx(b,{item:l,isActive:l.id===s},l.id))})},i.id))})]})]})},F={h2:({children:t,id:s})=>e.jsx(v,{level:2,id:s,registerSidebarAnchor:!0,children:t}),h3:({children:t,id:s})=>e.jsx(v,{level:3,id:s,registerSidebarAnchor:!0,children:t})},Z=({mdxComponent:t,file:s,frontmatter:a={},defaultOptions:r,tableOfContents:d,excerpt:n})=>{var i,l,h;const w=(i=R())==null?void 0:i.categoryLabel;let f=null;const k=E("");typeof window<"u"&&(f=`${window.location.origin}${k}`);const c=a.title,g=a.category??w,$=a.toc===!1||(r==null?void 0:r.toc)===!1,A=((l=d.find(o=>o.depth===1))==null?void 0:l.value)??c,C=a.disablePager??(r==null?void 0:r.disablePager)??!1,j=((h=d.find(o=>o.depth===1))==null?void 0:h.children)??d.filter(o=>o.depth===2),M=!$&&j.length>0,{prev:m,next:p}=L();return x.useEffect(()=>{},[s]),e.jsxs("div",{className:"grid grid-cols-1 xl:grid-cols-[--sidecar-grid-cols] gap-8 justify-between","data-pagefind-filter":"section:markdown","data-pagefind-meta":"section:markdown",children:[e.jsxs(P,{children:[e.jsx("title",{children:A}),n&&e.jsx("meta",{name:"description",content:n}),f&&e.jsx("link",{rel:"canonical",href:f})]}),e.jsxs("div",{className:u(q,"max-w-full xl:w-full xl:max-w-3xl flex-1 flex-shrink pt-[--padding-content-top] pb-[--padding-content-bottom]"),children:[(g||c)&&e.jsxs("header",{children:[g&&e.jsx(S,{children:g}),c&&e.jsx(v,{level:1,id:T(c),children:c})]}),e.jsx(t,{components:{..._(),...F}}),!C&&e.jsxs(e.Fragment,{children:[e.jsx("hr",{}),e.jsxs("div",{className:"not-prose flex flex-wrap items-center justify-between gap-2 lg:gap-8",children:[m?e.jsxs(y,{to:m.id,className:"flex flex-col items-stretch gap-2 flex-1 min-w-max border rounded px-6 py-4 text-start hover:border-primary/85 transition shadow-sm hover:shadow-md",title:m.label,children:[e.jsx("div",{className:"text-sm text-muted-foreground",children:"← Previous page"}),e.jsx("div",{className:"text-lg text-primary truncate",children:m.label})]}):e.jsx("div",{className:"flex-1"}),p?e.jsxs(y,{to:p.id,className:"flex flex-col items-stretch gap-2 flex-1 min-w-max border rounded px-6 py-4 text-end hover:border-primary/85 transition shadow-sm hover:shadow-md",title:p.label,children:[e.jsx("div",{className:"text-sm text-muted-foreground",children:"Next page →"}),e.jsx("div",{className:"text-lg text-primary truncate",children:p.label})]}):e.jsx("div",{className:"flex-1"})]})]})]}),e.jsx("div",{className:"hidden xl:block",children:M&&e.jsx(U,{entries:j})})]})};export{Z as MdxPage};
//# sourceMappingURL=MdxPage-D2rD1vC4-CQeIBrZ6.js.map
