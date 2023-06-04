export interface OrderDetail{ 
    id: number,
    idProduct: number,
    idOrder: number,
    quantity: number
}

export class OrderDetail {
    id!: number;
    idProduct!: number;
    idOrder!: number;
    quantity!: number
}