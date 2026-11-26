export interface ProgressDto {
    beltName: string;
    beltColor: string;
    rank: number;
    score: number;
    nextBeltName: string | null;
    nextBeltColor: string | null;
    nextBeltRequiredScore: number | null;
    canFightBoss: boolean;
    currentStreak: number;
    longestStreak: number;
    reviewedToday: number;
    dueCount: number;
}
