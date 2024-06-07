import { IEdificioModel } from "./iedificio-model";

export interface IEncargadoModel {
    email: string;
    nombre: string;
    apellido: string;
    contrasena: string;
    edificios: IEdificioModel[];
}
