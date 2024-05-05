import React from "react";
import { BrowserRouter, Route, Routes as Switch} from "react-router-dom";
import Login from './pages/Login';
import Alunos from "./pages/Alunos";
import NovoAluno from "./pages/NovoAluno";

export default function Routes(){
    return (
        <BrowserRouter>
            <Switch>
                <Route path="/" exact element={<Login />}/>
                <Route path="/alunos" element={<Alunos />}/> 
                <Route path="/aluno/novo/:alunoId" element={<NovoAluno />}/>         
            </Switch>
        </BrowserRouter>
    );
}
