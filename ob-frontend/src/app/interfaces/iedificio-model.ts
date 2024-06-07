import { IConstructoraModel } from "./iconstructora-model";
import { IDeptoModel } from "./idepto-model";

export interface IEdificioModel {
    nombre: string;
    direccion: string;
    ubicacion: string;
    empresaConstructora: IConstructoraModel;
    gastosComunes: number;
    deptos: IDeptoModel[];
}
