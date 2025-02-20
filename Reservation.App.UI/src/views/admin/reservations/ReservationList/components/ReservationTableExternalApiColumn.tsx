import { ExternalAPIsEnum } from '@/@types/api/importData'

import ExternalApiBadgeName from '@/components/shared/custom/ExternalApiBadgeName'

const ReservationTableExternalApiColumn = ({
    apis,
}: {
    apis: ExternalAPIsEnum[]
}) => {
    return (
        <div className="flex items-center w-full gap-1">
            {apis.length === 0
                ? '/'
                : apis.map((externalApiEnum) => (
                      <ExternalApiBadgeName
                          key={externalApiEnum}
                          showOnlyApiNameInTooltipLabel
                          externalApi={externalApiEnum}
                      />
                  ))}
        </div>
    )
}

export default ReservationTableExternalApiColumn
