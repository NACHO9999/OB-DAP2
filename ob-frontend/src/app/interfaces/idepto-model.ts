import { IDuenoModel } from "./idueno-model";

export interface IDeptoModel {
    Piso: number;
    Numero: number;
    Dueno: IDuenoModel | null;
    CantidadCuartos: number;
    CantidadBanos: number;
    ConTerraza: boolean;
    EdificioNombre: string;
    EdificioDireccion: string;
}
