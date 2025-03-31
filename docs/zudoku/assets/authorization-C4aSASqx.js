import{Z as r,j as e}from"./entry.client-CyU0adoz.js";const s="This API uses JWT tokens.",c=[{depth:1,value:"Authorization",id:"authorization"}],h=void 0;function o(t){const n={code:"code",h1:"h1",li:"li",ol:"ol",p:"p",pre:"pre",...r(),...t.components};return e.jsxs(e.Fragment,{children:[e.jsx(n.h1,{id:"authorization",children:"Authorization"}),`
`,e.jsx(n.p,{children:"This API uses JWT tokens."}),`
`,e.jsx(n.p,{children:"To authorize:"}),`
`,e.jsxs(n.ol,{children:[`
`,e.jsxs(n.li,{children:["Send a POST request to ",e.jsx(n.code,{inline:!0,children:"/api/auth/login"})," with your credentials."]}),`
`,e.jsx(n.li,{children:"Copy the returned JWT token."}),`
`,e.jsx(n.li,{children:"Use the token in the Authorization header for all protected endpoints:"}),`
`]}),`
`,e.jsx(n.pre,{children:e.jsx(n.code,{children:`Authorization: Bearer SuperSecretKey123456789!@#$%^&*()
`})})]})}function d(t={}){const{wrapper:n}={...r(),...t.components};return n?e.jsx(n,{...t,children:e.jsx(o,{...t})}):o(t)}export{d as default,s as excerpt,h as frontmatter,c as tableOfContents};
//# sourceMappingURL=authorization-C4aSASqx.js.map
