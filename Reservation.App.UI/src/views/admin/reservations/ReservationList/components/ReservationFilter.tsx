import {
    Button,
    DatePicker,
    Drawer,
    FormContainer,
    FormItem,
    Radio,
} from '@/components/ui'
import { Field, FieldProps, Form, Formik, FormikProps } from 'formik'
import { forwardRef, useRef, useState } from 'react'
import { HiFilter, HiOutlineFilter } from 'react-icons/hi'
import {
    getReservations,
    setFilterData,
    setTableData,
    useAppDispatch,
    useAppSelector,
} from '../store'
import { ExternalAPIsEnum } from '@/@types/api/importData'
// eslint-disable-next-line import/named
import { cloneDeep } from 'lodash'
import { useConfig } from '@/components/ui/ConfigProvider'
import {
    FilterParameters,
    ReservationStatusEnum,
} from '@/@types/api/reservation'
import getReservationStatusEnumName from '@/utils/getReservationStatusEnumName'
import adjustsTimeZoneOffset from '@/utils/adjustTimeZoneOffset'

const APIs = [
    { label: 'Исклучен', value: ExternalAPIsEnum.None },
    { label: 'Test', value: ExternalAPIsEnum.Test },
]

const ReservationStatuses = [
    ...Object.values(ReservationStatusEnum)
        .filter((status) => typeof status === 'number')
        .map((status) => ({
            label: getReservationStatusEnumName(status),
            value: status,
        })),
]

type FormModel = FilterParameters

type FilterFormProps = {
    onSubmitComplete?: () => void
}

const FilterForm = forwardRef<FormikProps<FormModel>, FilterFormProps>(
    ({ onSubmitComplete }, ref) => {
        const dispatch = useAppDispatch()

        const tableData = useAppSelector(
            (state) => state.reservationList.data.tableData
        )

        const filterData = useAppSelector(
            (state) => state.reservationList.data.filterData
        )

        const handleSubmit = (values: FormModel) => {
            const newTableData = cloneDeep(tableData)
            newTableData.pageNumber = 1

            onSubmitComplete?.()
            dispatch(setTableData(newTableData))
            dispatch(setFilterData(values))
            dispatch(getReservations({ ...newTableData, ...values }))
        }

        return (
            <Formik
                enableReinitialize
                innerRef={ref}
                initialValues={filterData}
                onSubmit={(values) => {
                    handleSubmit(values)
                }}
            >
                {({ values }) => (
                    <Form>
                        <FormContainer>
                            <FormItem className="mb-4">
                                <h6 className="mb-2">Дата на резервација</h6>
                                <Field name="created">
                                    {({ form }: FieldProps) => {
                                        const handleDateChange = (
                                            option: [Date | null, Date | null]
                                        ) => {
                                            const [startDate, endDate] = option
                                            const adjustedStartDate = startDate
                                                ? adjustsTimeZoneOffset(
                                                      startDate
                                                  )
                                                : null
                                            const adjustedEndDate = endDate
                                                ? adjustsTimeZoneOffset(endDate)
                                                : null

                                            form.setFieldValue(
                                                'createdStart',
                                                adjustedStartDate?.toISOString()
                                            )
                                            form.setFieldValue(
                                                'createdEnd',
                                                adjustedEndDate?.toISOString()
                                            )
                                        }

                                        return (
                                            <DatePicker.DatePickerRange
                                                placeholder="Одбери..."
                                                inputFormat="DD-MM-YYYY"
                                                singleDate={true}
                                                value={
                                                    values.createdStart &&
                                                    values.createdEnd
                                                        ? [
                                                              new Date(
                                                                  values.createdStart
                                                              ),
                                                              new Date(
                                                                  values.createdEnd
                                                              ),
                                                          ]
                                                        : [null, null]
                                                }
                                                onChange={handleDateChange}
                                            />
                                        )
                                    }}
                                </Field>
                            </FormItem>

                            <FormItem className="mb-4">
                                <h6 className="mb-2">Пристигнување</h6>
                                <Field name="CheckIn">
                                    {({ form }: FieldProps) => {
                                        const handleDateChange = (
                                            option: [Date | null, Date | null]
                                        ) => {
                                            const [startDate, endDate] = option
                                            const adjustedStartDate = startDate
                                                ? adjustsTimeZoneOffset(
                                                      startDate
                                                  )
                                                : null
                                            const adjustedEndDate = endDate
                                                ? adjustsTimeZoneOffset(endDate)
                                                : null

                                            form.setFieldValue(
                                                'checkInStart',
                                                adjustedStartDate?.toISOString()
                                            )
                                            form.setFieldValue(
                                                'checkInEnd',
                                                adjustedEndDate?.toISOString()
                                            )
                                        }

                                        return (
                                            <DatePicker.DatePickerRange
                                                placeholder="Одбери..."
                                                inputFormat="DD-MM-YYYY"
                                                singleDate={true}
                                                value={
                                                    values.checkInStart &&
                                                    values.checkInEnd
                                                        ? [
                                                              new Date(
                                                                  values.checkInStart
                                                              ),
                                                              new Date(
                                                                  values.checkInEnd
                                                              ),
                                                          ]
                                                        : [null, null]
                                                }
                                                onChange={handleDateChange}
                                            />
                                        )
                                    }}
                                </Field>
                            </FormItem>

                            <FormItem className="mb-8">
                                <h6 className="mb-2">Заминување</h6>
                                <Field name="CheckOut">
                                    {({ form }: FieldProps) => {
                                        const handleDateChange = (
                                            option: [Date | null, Date | null]
                                        ) => {
                                            const [startDate, endDate] = option
                                            const adjustedStartDate = startDate
                                                ? adjustsTimeZoneOffset(
                                                      startDate
                                                  )
                                                : null
                                            const adjustedEndDate = endDate
                                                ? adjustsTimeZoneOffset(endDate)
                                                : null

                                            form.setFieldValue(
                                                'checkOutStart',
                                                adjustedStartDate?.toISOString()
                                            )
                                            form.setFieldValue(
                                                'checkOutEnd',
                                                adjustedEndDate?.toISOString()
                                            )
                                        }

                                        return (
                                            <DatePicker.DatePickerRange
                                                placeholder="Одбери..."
                                                inputFormat="DD-MM-YYYY"
                                                singleDate={true}
                                                value={
                                                    values.checkOutStart &&
                                                    values.checkOutEnd
                                                        ? [
                                                              new Date(
                                                                  values.checkOutStart
                                                              ),
                                                              new Date(
                                                                  values.checkOutEnd
                                                              ),
                                                          ]
                                                        : [null, null]
                                                }
                                                onChange={handleDateChange}
                                            />
                                        )
                                    }}
                                </Field>
                            </FormItem>

                            <FormItem className="mb-5">
                                <h6 className="mb-4">Екстерно API</h6>
                                <Field name="activeApiAtTimeOfReservation">
                                    {({ field, form }: FieldProps) => (
                                        <Radio.Group
                                            key={1}
                                            vertical
                                            value={
                                                values.activeApiAtTimeOfReservation
                                            }
                                            className="ml-2 text-sm"
                                            onChange={(options) =>
                                                form.setFieldValue(
                                                    field.name,
                                                    options
                                                )
                                            }
                                        >
                                            {APIs.map((api) => (
                                                <Radio
                                                    key={api.value}
                                                    className="!mb-2"
                                                    name={field.name}
                                                    value={api.value}
                                                >
                                                    {api.label}
                                                </Radio>
                                            ))}
                                        </Radio.Group>
                                    )}
                                </Field>
                            </FormItem>

                            <FormItem>
                                <h6 className="mb-4">Статус на резервација</h6>
                                <Field name="reservationStatus">
                                    {({ field, form }: FieldProps) => (
                                        <Radio.Group
                                            key={2}
                                            vertical
                                            value={values.reservationStatus}
                                            className="ml-2"
                                            onChange={(options) =>
                                                form.setFieldValue(
                                                    field.name,
                                                    options
                                                )
                                            }
                                        >
                                            {ReservationStatuses.map(
                                                (status) => (
                                                    <Radio
                                                        key={status.value}
                                                        className="!mb-2"
                                                        name={field.name}
                                                        value={status.value}
                                                    >
                                                        {status.label}
                                                    </Radio>
                                                )
                                            )}
                                        </Radio.Group>
                                    )}
                                </Field>
                            </FormItem>
                        </FormContainer>
                    </Form>
                )}
            </Formik>
        )
    }
)

type DrawerFooterProps = {
    // eslint-disable-next-line @typescript-eslint/no-explicit-any
    onSaveClick: (event: any) => void
    // eslint-disable-next-line @typescript-eslint/no-explicit-any
    onCancel: (event: any) => void
}

const DrawerFooter = ({ onSaveClick, onCancel }: DrawerFooterProps) => {
    return (
        <div className="w-full text-right">
            <Button size="sm" className="mr-2" onClick={onCancel}>
                Откажи
            </Button>
            <Button size="sm" variant="solid" onClick={onSaveClick}>
                Потврди
            </Button>
        </div>
    )
}

const ReservationFilter = () => {
    const formikRef = useRef<FormikProps<FormModel>>(null)

    const filterData = useAppSelector(
        (state) => state.reservationList.data.filterData
    )

    const [isOpen, setIsOpen] = useState(false)

    const { themeColor } = useConfig()

    const isThereFilterData = Object.values(filterData).some((f) => !!f)

    const openDrawer = () => {
        setIsOpen(true)
    }

    const onDrawerClose = () => {
        setIsOpen(false)
    }

    const formSubmit = () => {
        formikRef.current?.submitForm()
    }

    return (
        <>
            <Button
                size="sm"
                className="block md:inline-block "
                icon={
                    isThereFilterData ? (
                        <HiFilter className={`text-${themeColor}-500`} />
                    ) : (
                        <HiOutlineFilter />
                    )
                }
                onClick={() => openDrawer()}
            >
                Филтер
            </Button>
            <Drawer
                title="Филтер"
                isOpen={isOpen}
                footer={
                    <DrawerFooter
                        onCancel={onDrawerClose}
                        onSaveClick={formSubmit}
                    />
                }
                onClose={onDrawerClose}
                onRequestClose={onDrawerClose}
            >
                <FilterForm ref={formikRef} onSubmitComplete={onDrawerClose} />
            </Drawer>
        </>
    )
}

FilterForm.displayName = 'FilterForm'

export default ReservationFilter
