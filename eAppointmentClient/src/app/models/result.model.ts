export class ResultModel<T>{
    data?: T;
    errorMessage?: string[];
    isSuccesful: boolean = true;
    statusCode: number = 200;
}