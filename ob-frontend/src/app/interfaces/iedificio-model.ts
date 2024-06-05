import { IConstructoraModel } from "./iconstructora-model";
import { IDeptoModel } from "./idepto-model";

export interface IEdificioModel {
    Nombre: string;
    Direccion: string;
    Ubicacion: string;
    EmpresaConstructora: IConstructoraModel;
    GastosComunes: number;
    Deptos: IDeptoModel[];
}
