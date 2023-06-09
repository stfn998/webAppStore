export interface OrderDetail{ 
    productId: number,
    orderId: number,
    customerId: number,
    quantity: number
}

export class OrderDetail {
    productId!: number;
    orderId!: number;
    customerId!: number;
    quantity!: number
}