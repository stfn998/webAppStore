export interface OrderDetail{ 
    customerId: number,
    productId: number,
    orderId: number,
    quantity: number
}

export class OrderDetail {
    customerId!: number;
    productId!: number;
    orderId!: number;
    quantity!: number
}