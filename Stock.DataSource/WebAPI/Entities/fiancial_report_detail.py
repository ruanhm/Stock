from typing import Optional

class FinancialReportDetail:
    def __init__(
            self,
            ItemName:Optional[str],
            ItemValue:Optional[float],
            ):
        self.ItemName=ItemName
        self.ItemValue=ItemValue