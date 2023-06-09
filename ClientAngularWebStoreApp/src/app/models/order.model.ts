import { OrderDetail } from "./orderdetail.model";
import { Product } from "./product.model";

export interface Order{ 
    id: string,
    customerId: number,
    deliveryAddress: string,
    comment: string,
    orderDate: string,
    deliveryTime: string,
    canCancel: boolean,
    shippingCost: number,
    idOrder: number,
    totalPrice: number,
    orderDetails: OrderDetail[]
}

export class Order {
    id!: string;
    customerId!: number;
    deliveryAddress!: string;
    comment!: string;
    orderDate!: string;
    deliveryTime!: string;
    canCancel!: boolean;
    shippingCost!: number;
    idOrder!: number;
    totalPrice!: number;
    orderDetails!: OrderDetail[];
}