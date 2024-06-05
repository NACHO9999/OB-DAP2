import { ICategoriaModel } from "./icategoria-model";
import { IDeptoModel } from "./idepto-model";
import { IUserCreate } from "./user-create";

export interface ISolicitudModel {
    Id: string;
    PerMan: IUserCreate | null;
    Descripcion: string;
    Depto: IDeptoModel;
    Categoria: ICategoriaModel;
    FechaCreacion: Date;
    FechaCierre: Date | null;
}
