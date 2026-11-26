export interface ReviewDto {
    id?: number;
    cardId: number;
    answer: string;
    grade: number;
    isCorrect?: boolean;
    reviewedAt?: string;
}
