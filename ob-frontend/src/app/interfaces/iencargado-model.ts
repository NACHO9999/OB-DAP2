import { IDeptoModel } from "./idepto-model";

export interface IEncargadoModel {
    Email: string;
    Nombre: string;
    Apellido: string;
    Contrasena: string;
    Deptos: IDeptoModel[];
}
